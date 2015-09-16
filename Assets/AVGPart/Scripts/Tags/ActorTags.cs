using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Sov.AVGPart
{

    public class ActorTagsUtility
    {
        static public float GetActorPositionX(string positionX)
        {
            switch (positionX)
            {
                case "center":
                    return Settings.Instance.Center_X;
                case "left":
                    return Settings.Instance.Left_X;
                case "right":
                    return Settings.Instance.Right_X;
                case "mid_left":
                    return Settings.Instance.Mid_Left_X;
                case "mid_right":
                    return Settings.Instance.Mid_Right_X;
                default: //center by default
                    return Settings.Instance.Center_X;
            }
            /*
            switch (positionX)
            {
                case "center":
                    return new Vector3(Settings.Center_X, y, z);
                case "left":
                    return new Vector3(Settings.Left_X, y, z);
                case "right":
                    return new Vector3(Settings.Right_X, y, z);
                case "mid_left":
                    return new Vector3(Settings.Mid_Left_X, y, z);
                case "mid_right":
                    return new Vector3(Settings.Mid_Right_X, y, z);
                default: //center by default
                    return new Vector3(Settings.Center_X, y, z);
            }*/
        }
        static public float GetActorPositionY()
        {
            return Settings.Instance.Actor_Y;
        }
        static public float GetActorPositionZ(string positionZ)
        {
            float z = Settings.Instance.Normal_Z;
            switch (positionZ)
            {
                case "Far":
                    z = Settings.Instance.Far_Z;
                    break;
                case "Near":
                    z = Settings.Instance.Near_Z;
                    break;
                case "Normal":
                    z = Settings.Instance.Normal_Z;
                    break;
                default:
                    z = Settings.Instance.Normal_Z;
                    break;
            }
            return z;
        }

        static public Vector3 GetActorPosition(string pos, string posZ)
        {
            float z = Settings.Instance.Normal_Z;
            float y = Settings.Instance.Actor_Y;

            switch (posZ)
            {
                case "Far":
                    z = Settings.Instance.Far_Z;
                    break;
                case "Near":
                    z = Settings.Instance.Near_Z;
                    break;
                case "Normal":
                    z = Settings.Instance.Normal_Z;
                    break;
                default:
                    z = Settings.Instance.Normal_Z;
                    break;
            }

            switch (pos)
            {
                case "center":
                    return new Vector3(Settings.Instance.Center_X, y, z);
                case "left":
                    return new Vector3(Settings.Instance.Left_X, y, z);
                case "right":
                    return new Vector3(Settings.Instance.Right_X, y, z);
                case "mid_left":
                    return new Vector3(Settings.Instance.Mid_Left_X, y, z);
                case "mid_right":
                    return new Vector3(Settings.Instance.Mid_Right_X, y, z);
                default: //center by default
                    return new Vector3(Settings.Instance.Center_X, y, z);
            }
        }
    }
    /*
     * tag = actor_new
     * 
     * <desc>
     * 预创建新的立绘, 默认为不激活状态
     * 
     * <params>
     * @name:       立绘的文件名，是一个prefab
     * @objname:    创建的GameObject的名称，默认为文件名
     * @path:       立绘存放路径，"Resources/Actor/Image"下的相对路径
     * @pos:        立绘显示的水平位置, 是一个enum, 有以下五种：
     *              {center, left, right, mid_left, mid_right}
     * @z_pos:      立绘显示的纵向位置，及近和远
     *              {far, near, normal}
     * @scale:      立绘的扩大倍数，1为原始
     * 
     * <sample>
     * [actor_new name=Sachi position=center]
     *
     */

    public class Actor_newTag: AbstractTag 
    {
        public Actor_newTag()
        {
            _defaultParamSet = new Dictionary<string, string>() {
                { "objname", ""         },
                { "name",    ""         },
                { "path",    ""         },
              //{ "x",       "0"        },
              //{ "y",       "0"        },
              //{ "z",       "0"        },
              //{ "show",    "false"    },
              //{ "fade",    "false"    },
              //{ "fadetime","0"        },
              //{ "pos",     "center"   },
              //{ "z_pos",   "normal"   },
                { "scale",   "1"        }
            };

            _vitalParams = new List<string>() {
                "name",
                "path"
            };
        }

        public override void Excute()
        {
            Debug.LogFormat("Create Actor: {0}", Params["name"]);
            //set objname
            if(Params["objname"] == "")
            {
                Params["objname"] = Params["name"];
            }

            //set path
            string path = Params["path"];
            path = Settings.Instance.ACTOR_IMAGE_PATH + path;
            Params["path"] = path;

            //set position
            /*
            Vector3 pos = GetActorPosition(Params["pos"], Params["z_pos"]);
            Params["x"] = pos.x.ToString();
            Params["y"] = pos.y.ToString();
            Params["z"] = pos.z.ToString();
            */
            
            ImageInfo info = new ImageInfo(Params);
            //set position
           // info.Position = new Vector2(Screen.width / 2,
             //                           Screen.height / 2);

            ActorObject ao = ImageManager.Instance.CreateObject<ActorObject, ImageInfo>(info);

            //base.Excute();
        }


    }

    /* 
     * tag = enteractor
     * 
     * <desc>
     * 立绘出场
     * 
     * <params>
     * @name:       the name of the actor
     * @pos:        立绘显示的水平位置, 是一个enum, 有以下五种, 默认为center
     *              {center, left, right, mid_left, mid_right}
     * @z_pos:      立绘显示的纵向位置，及近和远, 默认为normal
     *              {far, near, normal}   
     * @fade:       是否淡入
     * @fadetime:   淡入时间
     * 
     * <sample>
     * [enteractor name=Sachi position=center fade=true]
     *
     */
    public class EnteractorTag: AbstractTag
    {
        public EnteractorTag()
        {
            _defaultParamSet = new Dictionary<string, string>() {
                { "name",    ""         },
                { "fade",    "false"    },
                { "fadetime","0.5"      },
                { "pos",     "center"   },
                { "z_pos",   "normal"   },
                { "scale",   "1"        }
            };

            _vitalParams = new List<string>() {
                "name",
            };
        }

        public override void Excute()
        {
            //base.Excute();

            //actor name
            string actorName = Params["name"];

            Debug.LogFormat("Enter Actor: {0}", actorName);

            //get actor
            ActorObject ao = ImageManager.Instance.GetCreatedObject<ActorObject>(actorName);
            if (ao == default(ActorObject))
                return;

            ao.Go.SetActive(true);

            Vector3 pos = ActorTagsUtility.GetActorPosition(Params["pos"],Params["z_pos"]);
            ao.SetPosition2D(new Vector2(pos.x, pos.y));
            ao.SetScale(pos.z);
            //ao.SetPosition3D(pos);
            ao.OnAnimationFinish = OnFinishAnimation;

            float time = float.Parse(Params["fadetime"]);

            
            if(Params["fade"] == "true")
            {
                Engine.Status.EnableNextCommand = false;
                ao.FadeIn(time);
            }
            else
            {               
                ao.FadeIn(0);
            }
        }



        public override void OnFinishAnimation()
        {
            if (Params["fade"] == "true")
            {
                Debug.Log("Finish Animation!");
                Engine.Status.EnableNextCommand = true;
                Engine.NextCommand();
            }
        }

        public override void After()
        {
            //base.After();
        }
    }

    /* 
     * tag = moveactor
     * 
     * <desc>
     * 移动立绘
     * 从当前位置移动到指定位置
     * 
     * <params>
     * @name:       the name of the actor
     * @pos:        立绘显示的水平位置, 是一个enum, 有以下五种, 默认为center
     *              {center, left, right, mid_left, mid_right}
     * @z_pos:      立绘显示的纵向位置，及近和远, 默认为normal
     *              {far, near, normal}   
     * @time:       移动的时间             
     * @scale:      立绘的大小 倍数
     * 
     * <sample>
     * [moveactor name=Maki pos=left]
     *
     */
    public class MoveactorTag : AbstractTag
    {
        public MoveactorTag()
        {
            
            _defaultParamSet = new Dictionary<string, string>() {
                { "name",    ""         },
                { "pos",     ""   },
                { "z_pos",   ""   },
                { "time",    "1"        },
                { "scale",   "1"        }
            };

            _vitalParams = new List<string>() {
                "name",
            };
        }

        public override void Excute()
        {
            //base.Excute();

            //actor name
            string actorName = Params["name"];

            Debug.LogFormat("Move Actor: {0}", actorName);

            //get actor
            ActorObject ao = ImageManager.Instance.GetCreatedObject<ActorObject>(actorName);
            if (ao == default(ActorObject))
                return;

            ao.Go.SetActive(true);

            float time = float.Parse(Params["time"]);

            bool isAnim = false;
            if (Params["pos"] != "")
            {
                isAnim = true;
                float x_move = ActorTagsUtility.GetActorPositionX(Params["pos"]);
                float y = ActorTagsUtility.GetActorPositionY();
                ao.MoveTo(new Vector2(x_move, y), time);
            }
            // Vector3 pos = ActorTagsUtility.GetActorPosition(Params["pos"],Params["z_pos"]);
            if (Params["z_pos"] != "")
            {
                isAnim = true;
                float z_scale = ActorTagsUtility.GetActorPositionZ(Params["z_pos"]);
                ao.ScaleTo(z_scale, time);
            }

            if (isAnim)
            {
                ao.OnAnimationFinish = OnFinishAnimation;
            }

            Engine.Status.EnableNextCommand = false;
        }


        public override void OnFinishAnimation()
        {
         //   if (Params["fade"] == "true")
         //   {
                Debug.Log("Move Finish!");
                Engine.Status.EnableNextCommand = true;
                Engine.NextCommand();
         //   }
        }

        public override void After()
        {
            //base.After();
        }
    }
}
