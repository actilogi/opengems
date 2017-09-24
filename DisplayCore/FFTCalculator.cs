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

namespace GEMS.Display.Core
{
    public class FFTCalculator
    {
        private Result1D timeDomainSource = null;
        private float[] fftOriginResult = null;

        private int fftDataLength = 0;
        private float step = 0;

        #region Public Properties

        public float Step
        {
            get { return step; }
        }

        public int FFTDataLength
        {
            get { return fftDataLength; }
        }

        public int PowOf2
        {
            get { return (int)Math.Log(fftDataLength,2); }
        }

        public int SourceDataLength
        {
            get { return timeDomainSource.Values.Count; }
        }

        #endregion

        public FFTCalculator ( Result1D source )
        {
            this.timeDomainSource = source;

            PreCalculate ( );
        }

        private void PreCalculate ( )
        {
            //Data length
            fftDataLength = (int)Math.Pow ( 2 , (int)( Math.Log ( timeDomainSource.Keys.Count , 2 ) + 1 ) );

            //Frequency unit
            step = 1 / (timeDomainSource.Keys[1] * fftDataLength);
        }

        public void Calculate ( )
        {
            float[] sdata = new float[fftDataLength];
            sdata.Initialize ( );
            Array.Copy ( timeDomainSource.Values.ToArray ( ) , sdata , timeDomainSource.Values.Count );

            fftOriginResult = FFT ( sdata , (ulong)fftDataLength , 1 );
        }


        public Result2D GetRealImageData ( float startFrequency , float endFrequency )
        {
            //Filter the data with start and end frequency 
            Result2D result = new Result2D ( DataDomainType.FrequencyDomain , "Frequency" , timeDomainSource.ValueLabel + " FFT-Real" , timeDomainSource.ValueLabel + " Imag" );
            for (int i = 0 ; i < fftDataLength ; i++)
            {
                float freq = i * step;
                if (freq >= startFrequency && freq <= endFrequency)
                {
                    result.Keys.Add ( freq );
                    result.Values1.Add ( fftOriginResult[i * 2] );
                    result.Values2.Add ( fftOriginResult[i * 2 + 1] );
                }
            }

            return result;
        }

        public Result2D GetAmpPhaseData ( float startFrequency , float endFrequency )
        {
            //Filter the data with start and end frequency
            Result2D result = new Result2D ( DataDomainType.FrequencyDomain , "Frequency" , timeDomainSource.ValueLabel + " FFT-Amp" , timeDomainSource.ValueLabel + " FFT-Phase" );
            for (int i = 0 ; i < fftDataLength ; i++)
            {
                float freq = i * step;
                if (freq >= startFrequency && freq <= endFrequency)
                {
                    result.Keys.Add ( freq );

                    float amp = (float)Math.Sqrt ( fftOriginResult[i * 2] * fftOriginResult[i * 2]  + fftOriginResult[i * 2 + 1] * fftOriginResult[i * 2 + 1] );

                    float phase = (float)(Math.Atan2 ( fftOriginResult[i * 2 + 1] , fftOriginResult[i * 2] ) * 180 / Math.PI);

                    result.Values1.Add ( amp );
                    result.Values2.Add ( phase );
                }
            }

            return result;
        }

        private static float[] FFT ( float[] sdata , ulong nn , int isign )
        {

            ulong n , mmax , m , j , istep , i;
            float wtemp , wr , wpr , wpi , wi , theta , tempr , tempi;

            n = nn << 1;

            float[] data = new float[n + 1];
            for (i = 0 ; i < (ulong)sdata.Length ; i++)
            {
                data[i * 2 + 1] = sdata[i];
            }

            j = 1;
            for (i = 1 ; i < n ; i += 2)
            {
                if (j > i)
                {
                    Swap ( ref data[j] , ref data[i] );
                    Swap ( ref data[j + 1] , ref data[i + 1] );
                }
                m = n >> 1;
                while (m >= 2 && j > m)
                {
                    j -= m;
                    m >>= 1;
                }
                j += m;
            }

            mmax = 2;
            while (n > mmax)
            {
                istep = mmax << 1;
                theta = (float)( isign * ( Math.PI * 2.0 / mmax ) );
                wtemp = (float)( Math.Sin ( 0.5 * theta ) );
                wpr = -2.0f * wtemp * wtemp;
                wpi = (float)Math.Sin ( theta );
                wr = 1.0f;
                wi = 0.0f;
                for (m = 1 ; m < mmax ; m += 2)
                {
                    for (i = m ; i <= n ; i += istep)
                    {
                        j = i + mmax;
                        //if ( ( i == 0 ) || ( j == 0 ) ) {
                        //    //qDebug( "ERROR" );
                        //}
                        tempr = wr * data[j] - wi * data[j + 1];
                        tempi = wr * data[j + 1] + wi * data[j];
                        data[j] = data[i] - tempr;
                        data[j + 1] = data[i + 1] - tempi;
                        data[i] += tempr;
                        data[i + 1] += tempi;
                    }
                    wr = ( wtemp = wr ) * wpr - wi * wpi + wr;
                    wi = wi * wpr + wtemp * wpi + wi;
                }
                mmax = istep;
            }

            return data;
        }

        private static void Swap ( ref float data1 , ref float data2 )
        {
            float temp = data1;
            data1 = data2;
            data2 = temp;
        }
    }
}
