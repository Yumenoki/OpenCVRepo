using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using OpenCvSharp;
using OpenCvSharp.Demo;
using UnityEngine.Diagnostics;
using UnityEngine.UI;
using Rect = OpenCvSharp.Rect;
using System.IO;


public class FaceDetect : MonoBehaviour
{
    public int webCamIndex = 0;
    private WebCamTexture _webCamTexture;
    private CascadeClassifier cascade; //der ObjectDetector
    private OpenCvSharp.Rect myFace;

    private Point mouth;

    public Vector2 faceLocation;

    public TextAsset faceCascade;
    public TextAsset eyes;
    public TextAsset shapes;
    //private FaceProcessorLive<WebCamTexture> landmarks; 
    private ShapePredictor sp;
    private ShapePredictor lol;

    //private Mat grayMat;
    //private VectorOfPoint2f shapeInFace;
    //private Ptr<ShapePredictor> facemark;
    

    private void Start()
    {
        //Webcambild in Webcam Textur speichern
        WebCamDevice[] devices = WebCamTexture.devices;
        _webCamTexture = new WebCamTexture(devices[webCamIndex].name);
        _webCamTexture.Play();

        //FaceDetection xml  
        cascade = new CascadeClassifier(Application.dataPath + @"/haarcascade_frontalface_default.xml");
        
        byte[] shapeDat = shapes.bytes;

        //landmarks = new FaceProcessorLive<WebCamTexture>();
        //landmarks.Initialize(faceCascade.text, eyes.text, shapes.bytes);
     
        sp = new ShapePredictor();
        sp.LoadData(shapeDat);
        
    }

    void Update()
    {
        //aus Webcam Textur OpenCV-Material machen, um current frame zum auswerten zu speichern
        Mat frame = OpenCvSharp.Unity.TextureToMat(_webCamTexture);
        findNewFace(frame);
        display(frame);
    }

    void findNewFace(Mat frame)
    {
        //frame schwarzweiß machen
        //Cv2.CvtColor(frame, grayMat, ColorConversionCodes.BGR2GRAY);
       
        var faces = cascade.DetectMultiScale(frame, 1.1, 2, HaarDetectionType.ScaleImage);
        
        if (faces.Length == 1)
        {
            faceLocation = new Vector2(faces[0].Location.X, faces[0].Location.Y);
            
            myFace = faces[0]; //Location und Size des ersten Gesichts 
            findLandmarks(frame, myFace);
        }
    }

    void findLandmarks(Mat frame, Rect face)
    {

        //predictor in NumPy array umwandeln??  

        //landmarks.MarkDetected();
        
        if (sp.DetectLandmarks(frame, face)[63] != null)
        {           
            mouth.X = sp.DetectLandmarks(frame, face)[63].X;
            mouth.Y = sp.DetectLandmarks(frame, face)[63].Y;
        }

    }

    void display(Mat frame)
    {
        if (myFace != null)
        {
            frame.Circle(myFace.Center, 90, Scalar.Red, 2);
            frame.Circle(mouth, 20, Scalar.Pink, 2);
        }
        //Webcam Bild mit neuem Kreis auf UI Bild abbilden (Open CV Material muss dafür wieder Textur werden)
        Texture frameToTexture = OpenCvSharp.Unity.MatToTexture(frame);
        gameObject.GetComponent<RawImage>().texture = frameToTexture;
    }
}
