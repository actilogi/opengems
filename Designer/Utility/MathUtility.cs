/*
 OpenGEMS is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.

 OpenGEMS is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

  Copyright 2007 by Computer and Communication Unlimited
*/

using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using GEMS.Designer.Models.GeometryOperations;

namespace GEMS.Designer.Utility
{
    public class MathUtility
    {  

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="num"></param>
        /// <returns></returns>        
        public static double[] GetPulseSerials(double fmin,
            double fmax,
            int lossness,
            PluseType pluseType, 
            int num, ref double tao,out double[] tsteps)
        {
            double t0 = 0.0;
            CalculateTaoT0(fmin, fmax, lossness, pluseType, ref tao, ref t0);

            //We should compute the dt by ourself
            double dt = (double)(t0 * 2.0 / num);

            return ComputePulseData(pluseType,dt, num, tao, t0,out tsteps);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="num"></param>
        /// <returns></returns>        
        public static double[] GetPulseSerials(double fmin,
            double fmax,
            int lossness,
            PluseType pluseType,
            float dt, int num, ref double tao,out double[] tsteps)
        {
            double t0 = 0.0;
            CalculateTaoT0(fmin, fmax, lossness, pluseType, ref tao, ref t0);

            //Determine the choose of num
            int n;
            n = (int)(t0 * 2.0 / dt);
            if (num < n)
            {
                num = n;
            }

            return ComputePulseData(pluseType, dt, num, tao, t0,out tsteps);
        }

        private static double[] ComputePulseData(
            PluseType pluseType,double dt, int num, double tao, double t0,out double[] tsteps)
        {
            tsteps = new double[num];
            double[] data = new double[num];

            double t, v;

            double maxv = -1.0;

            switch (pluseType)
            {
                case PluseType.Gaussian:
                    {
                        for (int i = 0; i < num; i++)
                        {
                            t = dt * i;
                            v = (t - t0) / tao;
                            v = v * v;
                            v = Math.Exp(-v);
                            if (maxv < Math.Abs(v))
                            {
                                maxv = Math.Abs(v);
                            }
                            data[i] = v;
                            tsteps[i] = t;
                        }
                        break;
                    }
                case PluseType.Differentiated_Gaussian:
                    {
                        for (int i = 0; i < num; i++)
                        {
                            t = dt * i;
                            v = (t - t0) / tao;
                            v = v * v;
                            v = Math.Exp(-v);
                            v = -2.0 * (t - t0) * v;
                            v = v / (tao * tao);
                            if (maxv < Math.Abs(v))
                            {
                                maxv = Math.Abs(v);
                            }
                            data[i] = v;
                            tsteps[i] = t;
                        }
                        break;
                    }
                default:
                    break;
            }


            for (int i = 0; i < num; i++)
            {
                data[i] = data[i] / maxv;
            }

            return data;
        }


        public static double[] ComputeFrequencyData(
            PluseType pluseType, double tao, double fmax,int num, out double[] fsteps)
        {
            fsteps = new double[num];
            double[] data = new double[num];

            double maxv = -1.0;

            switch (pluseType)
            {
                case PluseType.Gaussian:
                    {
                        for (int i = 0; i < num; i++)
                        {
                            fsteps[i] = fmax * 2 / num * i / 10e8;
                            data[i] = Math.Exp(-Math.Pow(Math.PI * tao * fsteps[i] * 10e8, 2.0));
                        }
                        break;
                    }
                case PluseType.Differentiated_Gaussian:
                    {
                        for (int i = 0; i < num; i++)
                        {
                            fsteps[i] = fmax * 2 / num * i / 10e8;
                            data[i] = fsteps[i] * 10e9 * Math.Exp(-Math.Pow(Math.PI * tao * fsteps[i] * 10e8, 2.0));

                            maxv = maxv > data[i] ? maxv : data[i];
                        }

                        for (int i = 0; i < num; i++)
                        {
                            data[i] = data[i] / maxv;
                        }
                        break;
                    }
                default:
                    break;
            }

            return data;
        }

        /// <summary>
        /// Compute tao and to
        /// </summary>
        private static void CalculateTaoT0(double fmin, 
            double fmax, 
            int lossness,
            PluseType pluseType,
            ref double tao,
            ref double t0)
        {
            const double crt = 4.0;
            double loss = 1.0 / Math.Pow(10.0, lossness / 20.0);

            switch (pluseType)
            {
                case PluseType.Gaussian:
                    {
                        tao = Math.Sqrt(-Math.Log(loss)) / (Math.PI * fmax);
                        t0 = Math.Sqrt(crt * Math.Log(10.0)) * tao;
                        break;
                    }
                case PluseType.Differentiated_Gaussian:
                    {
                        double[] c = new double[2] { 0, 0 };
                        c[0] = Math.PI * Math.PI * fmax * fmax;
                        c[1] = Math.Log(loss / (Math.Sqrt(2.0) * Math.PI * fmax)) - 0.5;
                        tao = NewtonSolver(c, 1.0);

                        t0 = tao / Math.Sqrt(2.0) * 1.001;
                        c[0] = 1.0 / (tao * tao);
                        c[1] = -crt * Math.Log(10.0) + Math.Log(tao / Math.Sqrt(2.0)) - 0.5;
                        t0 = NewtonSolver(c, t0);

                        break;
                    }
                default:
                    break;
            }

        }

        private static double NewtonSolver(double[] args, double x)
        {
            double tolerance = 1e-4;
            double xo, y, dy;

            do
            {
                xo = x;
                y = DifferentiatedGaussian(args, x);
                dy = DdifferentiatedGaussian(args, x);
                x = x - y / dy;
            }
            while ((Math.Abs(x - xo) / (Math.Abs(x) + Math.Abs(xo))) > tolerance);

            return x;
        }

        private static double DifferentiatedGaussian(double[] c, double x)
        {
            double f;

            f = c[0] * x * x + c[1] - Math.Log(x);

            return f;
        }

        private static double DdifferentiatedGaussian(double[] c, double x)
        {
            double f;

            f = 2.0 * c[0] * x - 1.0 / x;

            return f;
        }


        private static void Swap(ref double data1, ref double data2)
        {
            double temp = data1;
            data1 = data2;
            data2 = temp;
        }


        /// <summary>
        /// Compute the timedelay with the phase and phase frequency
        /// timedelay's unit is second, phase's is degreee, frequency's is Hz
        /// </summary>
        public static double ComputeTimeDelay(double phase, double frequency)
        {
            if (frequency > 0.0)
                return phase / (frequency * 360);
            else
                return 0.0;
        }

        /// <summary>
        /// Compute the phase with the timedelay and phase frequency
        /// timedelay's unit is second, phase's is degreee, frequency's is Hz
        /// </summary>
        public static double ComputePhase(double time, double frequency)
        {
            return time * frequency * 360;
        }


        /// <summary>
        /// Find the boundary 
        /// </summary>
        public static List<Point> GetBoundary ( int n , int m , int i0 , int j0 , int[,] fMat , out bool rflag )
        {
            int i = 0, j = 0;

            bool flag = false;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < m; j++)
                {
                    if (fMat[i, j] == 2)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag) break;
            }

            rflag = false;
            int[,] pt = new int[3, 2];
            int[] ptIN = new int[2];
            int k = 0;

            List<Point> roundList = new List<Point>();

            while (flag)
            {
                Point pos = new Point();
                pos.X = i + i0;
                pos.Y = j + j0;
                roundList.Add(pos);

                fMat[i, j] = 1;

                if (k < 3)
                {
                    pt[k, 0] = i;
                    pt[k, 1] = j;

                    k++;

                    if (k == 3)
                    {
                        if ((pt[1, 0] > 0) && (fMat[pt[1, 0] - 1, pt[1, 1]] == 2))
                        {
                            ptIN[0] = pt[1, 0] - 1;
                            ptIN[1] = pt[1, 1];
                        }
                        if ((pt[1, 0] < n - 1) && (fMat[pt[1, 0] + 1, pt[1, 1]] == 2))
                        {
                            ptIN[0] = pt[1, 0] + 1;
                            ptIN[1] = pt[1, 1];
                        }
                        if ((pt[1, 1] > 0) && (fMat[pt[1, 0], pt[1, 1] - 1] == 2))
                        {
                            ptIN[0] = pt[1, 0];
                            ptIN[1] = pt[1, 1] - 1;
                        }
                        if ((pt[1, 1] < m - 1) && (fMat[pt[1, 0], pt[1, 1] + 1] == 2))
                        {
                            ptIN[0] = pt[1, 0];
                            ptIN[1] = pt[1, 1] + 1;
                        }
                        if ((pt[1, 0] > 0) && (pt[1, 1] > 0) && (fMat[pt[1, 0] - 1, pt[1, 1] - 1] == 2))
                        {
                            ptIN[0] = pt[1, 0] - 1;
                            ptIN[1] = pt[1, 1] - 1;
                        }
                        if ((pt[1, 0] < n - 1) && (pt[1, 1] > 0) && (fMat[pt[1, 0] + 1, pt[1, 1] - 1] == 2))
                        {
                            ptIN[0] = pt[1, 0] + 1;
                            ptIN[1] = pt[1, 1] - 1;
                        }
                        if ((pt[1, 0] < n - 1) && (pt[1, 1] < m - 1) && (fMat[pt[1, 0] + 1, pt[1, 1] + 1] == 2))
                        {
                            ptIN[0] = pt[1, 0] + 1;
                            ptIN[1] = pt[1, 1] + 1;
                        }
                        if ((pt[1, 0] > 0) && (pt[1, 1] < m - 1) && (fMat[pt[1, 0] - 1, pt[1, 1] + 1] == 2))
                        {
                            ptIN[0] = pt[1, 0] - 1;
                            ptIN[1] = pt[1, 1] + 1;
                        }

                        double x, y;
                        x = pt[0, 0] - ptIN[0];
                        y = pt[0, 1] - ptIN[1];
                        Vector3 v1 = new Vector3((float)x, (float)y, 0.0f);
                        x = pt[2, 0] - ptIN[0];
                        y = pt[2, 1] - ptIN[1];
                        Vector3 v2 = new Vector3((float)x, (float)y, 0.0f);
                        Vector3 v3 = Vector3.Cross(v1, v2);
                        if (v3.Z < 0)
                        {
                            rflag = false;
                        }
                        else
                        {
                            rflag = true;
                        }
                    }
                }

                if ((i < n - 1) && (fMat[i + 1, j] == 2) && onBoundary(n, m, fMat, i + 1, j))
                {
                    i++;
                    continue;
                }
                if ((j < m - 1) && (fMat[i, j + 1] == 2) && onBoundary(n, m, fMat, i, j + 1))
                {
                    j++;
                    continue;
                }
                if ((i > 0) && (fMat[i - 1, j] == 2) && onBoundary(n, m, fMat, i - 1, j))
                {
                    i--;
                    continue;
                }
                if ((j > 0) && (fMat[i, j - 1] == 2) && onBoundary(n, m, fMat, i, j - 1))
                {
                    j--;
                    continue;
                }

                flag = false;
            }

            return roundList;
        }

        private static bool onBoundary(int n, int m, int[,] fMat, int i, int j)
        {
            if ((i == 0) || (i == n - 1) || (j == 0) || (j == m - 1)) return true;
            if ((i > 0) && (fMat[i - 1, j] == 0)) return true;
            if ((i < m - 1) && (fMat[i + 1, j] == 0)) return true;
            if ((j > 0) && (fMat[i, j - 1] == 0)) return true;
            if ((j < m - 1) && (fMat[i, j + 1] == 0)) return true;
            if ((i > 0) && (j > 0) && (fMat[i - 1, j - 1] == 0)) return true;
            if ((i > 0) && (j < m - 1) && (fMat[i - 1, j + 1] == 0)) return true;
            if ((i < n - 1) && (j > 0) && (fMat[i + 1, j - 1] == 0)) return true;
            if ((i < n - 1) && (j < m - 1) && (fMat[i + 1, j + 1] == 0)) return true;

            return false;
        }
    }
}
