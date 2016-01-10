using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApexEngine.Math;

namespace ApexEngine.Terrain.SimplexTerrain
{
    public class WorleyNoise
    {
        private class Point
        {
            public float x, y, z;
        }

        private float[] DistanceArray = new float[3];

        public int DistanceFunction = 0;
        public int CombineDistanceFunction = 0;
        public int Seed = 3221;

        float CombinerFunc1(float[] array)
        {
            return array[0];
        }

        float CombinerFunc2(float[] array)
        {
            return array[1] - array[0];
        }

        float CombinerFunc3(float[] array)
        {
            return array[2] - array[0];
        }

        float EuclidianDistanceFunc(Vector3f p1, Vector3f p2)
        {
            return (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y)
                + (p1.z - p2.z) * (p1.z - p2.z);
        }

        float ManhattanDistanceFunc(Vector3f p1, Vector3f p2)
        {
            return System.Math.Abs(p1.x - p2.x) + System.Math.Abs(p1.y - p2.y) + System.Math.Abs(p1.z - p2.z);
        }

        float ChebyshevDistanceFunc(Vector3f p1, Vector3f p2)
        {
            Vector3f diff = new Vector3f(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
            return System.Math.Max(System.Math.Max(System.Math.Abs(diff.x), System.Math.Abs(diff.y)), System.Math.Abs(diff.z));
        }

        private static uint probLookup(uint value)
        {
            if (value < 393325350) return 1;
            if (value < 1022645910) return 2;
            if (value < 1861739990) return 3;
            if (value < 2700834071) return 4;
            if (value < 3372109335) return 5;
            if (value < 3819626178) return 6;
            if (value < 4075350088) return 7;
            if (value < 4203212043) return 8;
            return 9;
        }

        void insert(float[] arr, float value)
        {
            float temp;
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                if (value > arr[i]) break;
                temp = arr[i];
                arr[i] = value;
                if (i + 1 < arr.Length) arr[i + 1] = temp;
            }
        }

        private static uint lcgRandom(uint lastValue)
        {
            return (uint)((1103515245u * lastValue + 12345u) % 0x100000000u);
        }

        private const uint OFFSET_BASIS = 2166136261;
        private const uint FNV_PRIME = 16777619;

        private static uint hash(uint i, uint j, uint k)
        {
            return (uint)((((((OFFSET_BASIS ^ (uint)i) * FNV_PRIME) ^ (uint)j) * FNV_PRIME) ^ (uint)k) * FNV_PRIME);
        }

        float noise(float x, float y, float z)
        {
            var value = 0;

            uint lastRandom;
            uint numberFeaturePoints;
            Vector3f randomDiff = new Vector3f(0, 0, 0);
            Vector3f featurePoint = new Vector3f(0, 0, 0);
            Vector3f inputPt = new Vector3f(x, y, z);

            int cubeX, cubeY, cubeZ;

            for (var i = 0; i < DistanceArray.Length; i++)
            {
                this.DistanceArray[i] = 6666;
            }
            //1. Determine which cube the evaluation point is in
            int evalCubeX = (int)System.Math.Floor(x);
            int evalCubeY = (int)System.Math.Floor(y);
            int evalCubeZ = (int)System.Math.Floor(z);


            for (int i = -1; i < 2; ++i)
                for (int j = -1; j < 2; ++j)
                    for (int k = -1; k < 2; ++k)
                    {
                        cubeX = evalCubeX + i;
                        cubeY = evalCubeY + j;
                        cubeZ = evalCubeZ + k;

                        //2. Generate a reproducible random number generator for the cube
                        lastRandom = lcgRandom(hash((uint)(cubeX + this.Seed), (uint)(cubeY), (uint)(cubeZ)));
                        //3. Determine how many feature points are in the cube
                        numberFeaturePoints = probLookup(lastRandom);
                        //4. Randomly place the feature points in the cube
                        for (uint l = 0; l < numberFeaturePoints; ++l)
                        {
                            lastRandom = lcgRandom(lastRandom);
                            randomDiff.X = (float)lastRandom / 0x100000000;

                            lastRandom = lcgRandom(lastRandom);
                            randomDiff.Y = (float)lastRandom / 0x100000000;

                            lastRandom = lcgRandom(lastRandom);
                            randomDiff.Z = (float)lastRandom / 0x100000000;

                            featurePoint = new Vector3f(randomDiff.X + (float)cubeX, randomDiff.Y + (float)cubeY, randomDiff.Z + (float)cubeZ);

                            //5. Find the feature point closest to the evaluation point. 
                            //This is done by inserting the distances to the feature points into a sorted list
                            insert(DistanceArray, EuclidianDistanceFunc(inputPt, featurePoint));
                        }
                        //6. Check the neighboring cubes to ensure their are no closer evaluation points.
                        // This is done by repeating steps 1 through 5 above for each neighboring cube
                    }

            var color = CombinerFunc1(DistanceArray);
            if (color < 0) color = 0;
            if (color > 1) color = 1;

            return color;
        }

        public float Get1D(float x)
        {
            return noise(x, 0, 0);
        }

        public float Get2D(float x, float y)
        {
            return noise(x, y, 0);
        }

        public float Get3D(float x, float y, float z)
        {
            return noise(x, y, z);
        }

        public float Get4D(float x, float y, float z, float t)
        {
            throw new NotImplementedException();
        }
    }
}
