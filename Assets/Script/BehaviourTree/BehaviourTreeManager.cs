using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine.AI;

namespace BehaviourTrees
{
    
    // BehaviourTree本体
    
    public class BehaviourTreeManager : MonoBehaviour
    {

        BehaviourTreeInstance node;

        Animator anim;

        public GameObject target;

        public GameObject obj;

        public Transform Player;

        float speed;

        public Transform[] points;

        public Transform[] BuildingPoint;

        private int destPoint = 0;

        RaycastHit hit;

        NavMeshAgent agent;

        Vector3 pos;

        LayerMask layerMask;

        bool IsAttack;

        bool IsAttackBuilding;

        bool IsCombatStatus;

        bool IsSliding = false;

        bool IsWalk = false;

        bool IsStop = false;

        private float ShortSqrDistance = 60f;

        private float LongSqrDistance = 150f;

        void Start()
        {

            anim = GetComponent<Animator>();

            agent = GetComponent<NavMeshAgent>();

            float sqrDistanceToPlayer = Vector3.SqrMagnitude(transform.position - Player.position);

            Debug.Log(sqrDistanceToPlayer);

            // autoBraking を無効にすると、目標地点の間を継続的に移動します
            //  (つまり、エージェントは目標地点に近づいても
            // 速度をおとしません)
            agent.autoBraking = false;

            pos = new Vector3(0, 0, 10);

            speed = 0.0f;

            IsAttack = false;

            IsWalk = true;

            anim.SetBool("IsWalk", IsWalk);

            CreateNodes();
        }

        void CreateNodes()
        {
            var attackNode = new SequencerNode(new BehaviourTreeBase[] { new ActionNode(AttackEnemy) });

            var BuildingAttack = new SequencerNode(new BehaviourTreeBase[] { new ActionNode(AttackBuilding) });

            var PlayerAttack = new SequencerNode(new BehaviourTreeBase[] { attackNode });

            var discover = new SelectorNode(new BehaviourTreeBase[] { new ActionNode(PlayerDiscover), new ActionNode(BuildingDiscover) });

            var moveNode = new SelectorNode(new BehaviourTreeBase[] { new ActionNode(RightMove), discover });

            var rootNode = new SelectorNode(new BehaviourTreeBase[]{

                BuildingAttack,PlayerAttack,moveNode

                }
            );
            node = new BehaviourTreeInstance(rootNode);
            node.finishRP.Where(p => p != BehaviourTreeInstance.NodeState.READY).Subscribe(p => ResetCoroutineStart());
            node.finishRP.Value = BehaviourTreeInstance.NodeState.READY;
            node.Excute();
        }

        private ExecutionResult IsHpLessThan(BehaviourTreeInstance instance)
        {
            var playerhp = GetComponent<EnemyHpBarControl>().hp;

                if (IsAttack)
                {
                    //anim.SetTrigger("Initimidate");
                    Debug.Log("プレイヤーを発見しちゃった");
                    return new ExecutionResult(true);
                }
                else if (playerhp <= 0)
                {
                    Debug.Log("死んじゃった。");
                    //anim.SetTrigger("IsDie");
                    //return new ExecutionResult(false);
                }
            
            Debug.Log("プレイヤーを発見出来てない");
            return new ExecutionResult(false);
        }


        private ExecutionResult AttackEnemy(BehaviourTreeInstance instance)
        {
            float t = 0;
           
            if (t < 1)
            {
                t += Time.deltaTime;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Player.rotation, t);

            anim.SetBool("IsSliding", IsSliding);

            Debug.Log("敵を攻撃する");
            if (IsAttack)
            {
                float sqrDistanceToPlayer = Vector3.SqrMagnitude(transform.position - Player.position);
                Debug.Log(sqrDistanceToPlayer);
                if (sqrDistanceToPlayer < ShortSqrDistance)
                {
                    IsSliding = true;

                    IsWalk = false;
                }
                
                agent.destination = target.transform.position;
                return new ExecutionResult(true);
            }
            else if(!IsAttack)
            {

                return new ExecutionResult(false);
            }
           
            return new ExecutionResult(false);
        }


        private ExecutionResult PlayerDiscover(BehaviourTreeInstance instance)
        {
           
            if(Physics.Linecast(transform.position, target.transform.position, 1<<9))
            {
                //見えない
                Debug.Log("何も発見できていないよー。");
                IsAttack = false;
                return new ExecutionResult(false);
            }
           else 
            {
                //見える
                Debug.Log("プレイヤーを発見");
                IsAttack = true;
               
                return new ExecutionResult(true);
            }

            
        }

        private ExecutionResult BuildingDiscover(BehaviourTreeInstance instance)
        {
            if(obj!=null)
            {
                if (Physics.Linecast(transform.position, obj.transform.position, 1 << 9))
                {
                    //見える
                    Debug.Log("建物発見");
                    IsAttackBuilding = true;
                    return new ExecutionResult(true);
                }
                else
                {
                    //見えない
                    Debug.Log("何も発見できていないよー。");
                    IsAttackBuilding = false;
                    return new ExecutionResult(false);
                }
            }
            return new ExecutionResult(false);
        }

        private ExecutionResult RightMove(BehaviourTreeInstance instance)
        {

            IsWalk = true;

            //エージェントが現目標地点に近づいてきたら、
            //次の目標地点を選択します
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }

            Debug.Log("歩いてるよー");
            return new ExecutionResult(false);
        }

        private ExecutionResult LeftMove(BehaviourTreeInstance instance)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            return new ExecutionResult(true);
        }

        void GotoNextPoint()
        {
            // 地点がなにも設定されていないときに返します
            if (points.Length == 0)
                return;

            //エージェントが現在設定された目標地点に行くように設定します
            agent.destination = points[destPoint].position;

            // 配列内の次の位置を目標地点に設定し、
            // 必要ならば開始にもどります
            destPoint = (destPoint + 1) % points.Length;
        }

        private ExecutionResult AttackBuilding(BehaviourTreeInstance instance)
        {
           
            if (IsAttackBuilding)
            {
                Debug.Log("建物を攻撃する");
                agent.destination = obj.transform.position;
                return new ExecutionResult(true);
            }
            else if(!IsAttackBuilding)
            {
                return new ExecutionResult(false);
            }

            return new ExecutionResult(false);
        }

        private ExecutionResult DetermineBuilding(BehaviourTreeInstance instance)
        {
            if (transform.position == obj.transform.position)
            {
                Debug.Log("やったね！");

                IsAttackBuilding = false;

                return new ExecutionResult(false);
            }
            return new ExecutionResult(true);
        }
       
        void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.tag == "Player")
            {
                Debug.Log("攻撃したよー");
                IsAttack = false;
                
            }

            if (other.gameObject.tag == "Box")
            {
                Debug.Log("壊したよー");
                IsAttackBuilding = false;
                Destroy(obj);
            }
        }

        void ResetCoroutineStart()
        {
            StartCoroutine(WaitCoroutine());
        }


        IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(1.0f);
            node.Reset();
            CreateNodes();
        }
    }
}
