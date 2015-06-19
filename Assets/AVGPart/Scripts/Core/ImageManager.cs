using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Sov.AVGPart
{
    class ImageInfo: ObjectInfo
    {
        //Params
      //public  string  ObjName   = "";
      //public  string  Name      = "";
        public  string  Path      = "";
        public  Vector3 Position  = new Vector3(0, 0, 0);
  //    public  float   x         = 0.0f;
  //    public  float   y         = 0.0f;    
  //    public  float   z         = 0.0f;
        public  bool    Show      = false;
        public  bool    Fade      = false;
        public  float   Fadetime  = 0.0f;
        public  string  Root      = "";
        public  float   Scale     = 1;

        public ImageInfo(Dictionary<string, string> param)
        {
            if(param.ContainsKey("objname"))
            {
                ObjName = param["objname"];
            }
            if(param.ContainsKey("name"))
            {
                Name = param["name"];
            }
            if(param.ContainsKey("path"))
            {
                Path = param["path"];
            }
            if(param.ContainsKey("show"))
            {
                Show = bool.Parse(param["show"]);
            }
            if(param.ContainsKey("fadeTime"))
            {
                Fadetime = float.Parse(param["fadeTime"]);
            }
            if(param.ContainsKey("root"))
            {
                Root = param["root"];
            }
            if(param.ContainsKey("scale"))
            {
                Scale = float.Parse(param["scale"]);
            }
        }
    }
    class ImageManager
    {
        public static ImageManager Instance
        {
            get
            {
                if (_sharedImageManager == null)
                {
                    _sharedImageManager = new ImageManager();
                }
                return _sharedImageManager;
            }
        }

        static ImageManager _sharedImageManager = null;

        
       

        ImageManager()
        {
           // _createdImage = new Dictionary<string, ImageObject>();
         //   _objectInScene = new Dictionary<string, AbstractObject>();
        }
        /*
        public void ChangeImageWithFade(ImageObject io, string imageFileName, float fadeTime)
        {
            io.ChangeImage(imageFileName, fadeTime);
        }*/

        /*
        public void ChangeImageWithoutFade(string objectName, string imageFileName)
        {
            ImageObject oldImage = ImageObject.CreateWithSceneObject(objectName);
            oldImage.ChangeImage(imageFileName, 0f);
        }*/
        /*
        public void ChangeImageWithoutFade(ImageObject io, string imageFileName)
        {
            io.ChangeImage(imageFileName, 0f);
        }*/
        
    }
}
