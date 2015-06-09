using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Sov.AVGPart
{
    class ImageObject:AbstractObject
    {
        Image _image;

        /*
        public ImageObject(string objectName)
        {
        }
        */

        ImageObject()
        {
            
        }

        public Action OnAnimationFinish
        {
            private get;
            set;
        }
        public static ImageObject CreateWithSceneObject(string objectName)
        {
            ImageObject io = new ImageObject();
            io.Go = GameObject.Find(objectName);
            if (io.Go == null)
            {
                Debug.LogFormat("Cannot find object:{0}", objectName);
                return null;
            }
            io._image = io.Go.GetComponent<Image>();
            return io;
        }

        public static ImageObject CreateWithNewImage(string imageFileName, string objectName)
        {
            ImageObject io = new ImageObject();

            io.Go = new GameObject(objectName);

            io._image = io.Go.AddComponent<Image>();
            //create image
            Sprite i = Resources.Load<Sprite>(imageFileName);
            if (i == null)
            {
                Debug.LogFormat("Cannot load image file:{0}", imageFileName);
            }
            return io;
        }

        public void ChangeImage(string newImageFileName, float fadeTime)
        {
            Sprite i = Resources.Load<Sprite>(newImageFileName);
            _image.sprite = i;
            _image.color = new Color(255, 255, 255, 0);
            _image.DOFade(1, fadeTime).OnComplete(new TweenCallback(OnAnimationFinish));
         //   Sequence s = DOTween.Sequence();
       //     s.Append(t);

        //    if(OnAnimationFinish != null)
           //     s.AppendCallback(new TweenCallback(OnAnimationFinish));
        }
       
    }
}
