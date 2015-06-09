using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Sov.AVGPart
{

    class ImageManager
    {
        public ImageObject GetImageObjectInScene(string objectName)
        {
            ImageObject i = ImageObject.CreateWithSceneObject(objectName);
            return i;
        }
        public void SetImageAnimationFinishCallback(ImageObject image,Action callback)
        {
            image.OnAnimationFinish = callback;
        }
        
        public void ChangeImageWithFade(string objectName, string imageFileName, float fadeTime)
        {
            ImageObject oldImage = ImageObject.CreateWithSceneObject(objectName);
            oldImage.ChangeImage(imageFileName, fadeTime);
        }

        public void ChangeImageWithFade(ImageObject io, string imageFileName, float fadeTime)
        {
            io.ChangeImage(imageFileName, fadeTime);
        }

        public void ChangeImageWithoutFade(string objectName, string imageFileName)
        {
            ImageObject oldImage = ImageObject.CreateWithSceneObject(objectName);
            oldImage.ChangeImage(imageFileName, 0f);
        }

        public void ChangeImageWithoutFade(ImageObject io, string imageFileName)
        {
            io.ChangeImage(imageFileName, 0f);
        }
        /*
        public GameObject GetImageObject(string objectName)
        {
            return GameObject.Find(objectName);
        }
        */
    }
}
