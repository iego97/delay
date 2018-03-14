using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Efectos
{
    class Delay : ISampleProvider
    {
        int offsetTiempoMS;

        List<float> muestras = new List<float>();

        private ISampleProvider fuente;


        public Delay(ISampleProvider fuente)
        {
            this.fuente = fuente;
            offsetTiempoMS = 1000;
        }

        public WaveFormat WaveFormat
        {
            get
            {
               return fuente.WaveFormat;
            }
        }

        //Offset, numero de muestras leídas
        public int Read(float[] buffer, int offset, int count)
        {

            

            var read = fuente.Read(buffer, offset, count);
            float tiempoTranscurrido = 
                (float)muestras.Count / (float)fuente.WaveFormat.SampleRate;

            float tiempoTranscurridoMS = tiempoTranscurrido * 1000;
            int numMuestrasOffsetTiempo = 
                (int)(((float)offsetTiempoMS / 1000.0f) * (float)fuente.WaveFormat.SampleRate);

            if (tiempoTranscurridoMS > offsetTiempoMS)
            {
                for (int i = 0; i < read; i++)
                {
                    buffer[offset + 1] += muestras[muestras.Count+i-numMuestrasOffsetTiempo];
                }
            }

            for (int i = 0; i < buffer.Length; i++)
            {
                muestras.Add(buffer[i]);
            }

            return read;
        }
    }
}
