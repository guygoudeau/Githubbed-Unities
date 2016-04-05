using UnityEngine;
using System;
using System.Collections.Generic;

namespace FinateStateMachine
{
    ///
    struct Link<T>
    {
        public T from;
        public T to;
    }

    /// <summary>
    /// this is fsm
    /// </summary>
    /// <typeparam name="T">this needs to be a list of enums or some shit</typeparam>
    class FSM<T> : MonoBehaviour
    {
        private Dictionary<T, List<Link<T>>> dict = new Dictionary<T, List<Link<T>>>();

        T currentState;
        public Enum cState
        {
            get { return currentState as Enum; }
            
        }
        public FSM(T initial)
        {
            
         //   var v = Enum.GetValues(typeof(T));
         // look up how to find out all enums from a generic type
            AddState(initial);
            currentState = initial;
        }

        public bool AddState(T state)
        {
            if (dict.ContainsKey(state))
            {
                return false;
            }
            dict.Add(state, new List<Link<T>>());
            return true;
        }


        public bool AddTransition(T from, T to)
        {
            Link<T> lin = new Link<T>();
            lin.from = from;
            lin.to = to;
            if (dict[from].Contains(lin))
            {
                return false;
            }
            dict[from].Add(lin);
            return true;
        }

        public T Switch(T to)
        {
            Link<T> temp = new Link<T>();
            temp.from = currentState;
            temp.to = to;
            foreach (Link<T> l in dict[currentState])
            {
                if (l.Equals(temp))
                {
                    currentState = to;
                    return currentState;
                }
            }
            return currentState;
        }
    }
}
