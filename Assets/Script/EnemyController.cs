//--------------------------------------------------------------------------------------------------
// @file	EnemyContller.cs
// @brief	EnemyContllerの実装
// @author	matsuda
//--------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace StateMachine
{

    public enum EnemyState
    {
        Wander,
        Pursuit,
        Attack,
        Explode,
    }

    public class EnemyController : StateObjectBase<EnemyController, EnemyState>
    {

        public Transform player;

        Animator anim;

        float hp;

        private float speed = 10f;

        private float rotationSmooth = 1f;

        private float turretRotationSmooth = 0.8f;

        private float attackInterval = 2f;

        private float pursuitSqrDistance = 2500f;

        private float attackSqrDistance = 900f;

        private float margin = 50f;

        private float changeTargetSqrDistance = 40f;

        void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            // 始めにプレイヤーの位置を取得できるようにする

            player = GameObject.FindWithTag("Player").transform;



            hp = GetComponent<EnemyHpBarControl>().hp;

            anim = GetComponent<Animator>();


            // ステートマシンの初期設定

            stateList.Add(new StateWander(this));

            stateList.Add(new StatePursuit(this));

            //stateList.Add(new StateAttack(this));

            //stateList.Add(new StateExplode(this));


            stateMachine = new StateMachine<EnemyController>();

            ChangeState(EnemyState.Wander);

        }

        private class StateWander : State<EnemyController>
        {
            private Vector3 targetPosition;

            public StateWander(EnemyController owner) : base(owner) { }

            public override void Enter()
            {
                // 始めの目標地点を設定する

                targetPosition = GetRandomPositionOnLevel();
            }
            //Debug.Log("プレイヤーを発見出来てない");
            public override void Execute()

            {

                // プレイヤーとの距離が小さければ、追跡ステートに遷移

                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);

                if (sqrDistanceToPlayer < owner.pursuitSqrDistance - owner.margin)

                {

                    owner.ChangeState(EnemyState.Pursuit);

                }



                // 目標地点との距離が小さければ、次のランダムな目標地点を設定する

                float sqrDistanceToTarget = Vector3.SqrMagnitude(owner.transform.position - targetPosition);

                if (sqrDistanceToTarget < owner.changeTargetSqrDistance)

                {

                    targetPosition = GetRandomPositionOnLevel();

                }



                // 目標地点の方向を向く

                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - owner.transform.position);

                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);



                // 前方に進む

                owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);

            }

            public override void Exit() { }

            public Vector3 GetRandomPositionOnLevel()
            {

                float levelSize = 55f;

                return new Vector3(Random.Range(-levelSize, levelSize), 0, Random.Range(-levelSize, levelSize));

            }

        }

        private class StatePursuit : State<EnemyController>

        {

            public StatePursuit(EnemyController owner) : base(owner) { }



            public override void Enter() { }



            public override void Execute()

            {

                // プレイヤーとの距離が小さければ、攻撃ステートに遷移

                float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);

                if (sqrDistanceToPlayer < owner.attackSqrDistance - owner.margin)

                {

                    owner.ChangeState(EnemyState.Attack);

                }



                // プレイヤーとの距離が大きければ、徘徊ステートに遷移

                if (sqrDistanceToPlayer > owner.pursuitSqrDistance + owner.margin)

                {

                    owner.ChangeState(EnemyState.Wander);

                }



                // プレイヤーの方向を向く

                Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);

                owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);



                // 前方に進む

                owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);

            }



            public override void Exit() { }

        }




    }
}