using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Sov.AVGPart
{
    /*
     * AbstractObject
     * 
     * 引擎通过这个创建UI元素
     * 
     */
    class AbstractObject
    {
        /*
         * 定义元素的位置
         */

        public  GameObject Go;

        protected abstract class AbPosition
        {

        }

        public virtual void SetPosition3D(float x, float y, float z)
        {
            Go.transform.position = new Vector3(x, y, z);
        }

        public virtual void SetPosition2D(float x, float y)
        {
            Vector3 v3 = Go.transform.position;
            Go.transform.position = new Vector3(x, y, v3.z);
        }

        //public void SetPosition(A)
    }
}
