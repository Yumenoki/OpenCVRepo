using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class FaceDetect : MonoBehaviour
{
    private WebCamTexture _webCamTexture;
    private CascadeClassifier cascade; //der ObjectDetector
    private OpenCvSharp.Rect myFace;

    public Vector2 faceLocation;

    private void Start()
    {
        //Webcambild in Webcam Textur speichern
        WebCamDevice[] devices = WebCamTexture.devices;
        _webCamTexture = new WebCamTexture(devices[0].name);
        _webCamTexture.Play();

        //FaceDetection xml  
        cascade = new CascadeClassifier(Application.dataPath + @"/haarcascade_frontalface_default.xml");

    }

    void Update()
    {
        //Webcam Bild auf UI Bild abbilden
        //gameObject.GetComponent<RawImage>().texture = _webCamTexture;
        
        
        //aus Webcam Textur OpenCV-Material machen, um current frame zum auswerten zu speichern
        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);
        findNewFace(frame);
        //display(frame);
    }

    void findNewFace(Mat frame)
    {
        var faces = cascade.DetectMultiScale(frame, 1.1, 2, HaarDetectionType.ScaleImage);

        if (faces.Length == 1)
        {
            faceLocation = new Vector2(faces[0].Location.X, faces[0].Location.Y);
            Debug.Log(faces[0].Location);
            myFace = faces[0]; //Location und Size des ersten Gesichts 
        }
    }

    void display(Mat frame)
    {
        if (myFace != null)
        {
            frame.Circle(myFace.Center, 90, Scalar.Red, 2);
        }
        //Webcam Bild mit neuem Kreis auf UI Bild abbilden (Open CV Material muss dafür wieder Textur werden)
        Texture frameToTexture = OpenCvSharp.Unity.MatToTexture(frame);
        gameObject.GetComponent<RawImage>().texture = frameToTexture;
    }
}
