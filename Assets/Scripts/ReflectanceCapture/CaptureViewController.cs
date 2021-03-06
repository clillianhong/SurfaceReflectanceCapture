﻿using System;
using System.Collections.Generic;
using UnityEngine;
using MagicLeap;

namespace CaptureSystem
{
    public class CaptureViewController : MonoBehaviour
    {

        // collection of captures 
        private CaptureViewCollection _collection;

        //callback for capture creation event
        public event Action<string> OnCaptureCreated = null;

        //counter for generating capture IDs 
        private int nextIDNum;
        public CaptureViewCollection collection
        {
            get { return _collection; }
        }

        void Awake()
        {
            _collection = new CaptureViewCollection();

        }
        // Start is called before the first frame update
        void Start()
        {
            nextIDNum = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary> 
        /// Creates a capture view object and adds it to the capture collection 
        /// </summary> 
        /// <param name="texture">The texture containing the image taken.</param>
        /// <param name="camTransform">The transform of the camera, used to populate the copy transform struct</param>
        /// <param name="lightPos">The position of the point light source.</param>

        public void CreateCaptureView(Texture2D texture, Transform camTransform, Vector3 lightPos, Matrix4x4 projMat, Matrix4x4 worldToCam)
        {
            float thetaS = 0; //TODO: ACTUALLY CALCULATE THETA S 

            Transform newTransform = camTransform;

            Capture newCapture = new Capture("" + nextIDNum, texture, thetaS, camTransform, lightPos, projMat, worldToCam);
            nextIDNum++;
            _collection.captures.Add(newCapture.captureID, newCapture);
            Debug.Log("Capture added to collection with ID " + newCapture.captureID);
            OnCaptureCreated?.Invoke(newCapture.captureID);
        }


        /// <summary> 
        /// Finds the nearest neighbor capture to the point [position]
        /// </summary> 
        /// <param name="position">The point of interest for nearest neighbor.</param>
        public Capture NearestNeighbor(Vector3 position)
        {
            double[] testpt = { position.x, position.y, position.z };
            Debug.Log("Looking for nearest neighbor to: (" + testpt[0] + ", " + testpt[1] + ", " + testpt[2] + ")");


            if (_collection.kdTree != null)
            {
                var knearest = _collection.kdTree.NearestNeighbors(point: testpt, neighbors: 1);
                var nearest = knearest[0];
                Debug.Log("Nearest neighbor found at: (" + nearest.Item1[0] + ", " + nearest.Item1[1] + ", " + nearest.Item1[2] + ") with CAPTURE ID" + nearest.Item2.captureID);
                return nearest.Item2;
            }
            return null;

        }
    }

}
