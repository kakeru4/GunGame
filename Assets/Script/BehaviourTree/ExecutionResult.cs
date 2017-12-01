﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace BehaviourTrees
{

    public class ExecutionResult
    {

        public enum KIND_OF_RESULT
        {

            BOOLEAN,

            INTEGER

        }



        public KIND_OF_RESULT Kind;

        public bool BooleanResult;

        public int IntegerResult;



        public ExecutionResult(Boolean booleanResult)
        {

            Kind = KIND_OF_RESULT.BOOLEAN;

            this.BooleanResult = booleanResult;

        }



        public ExecutionResult(int integerResult)
        {

            Kind = KIND_OF_RESULT.INTEGER;

            this.IntegerResult = integerResult;

        }

    }

}