using SkiaSharp;

namespace Microcharts
{
    internal class Entry
    {
        private int v;

        public Entry(int v)
        {
            this.v = v;
        }

        public string Label { get; set; }
        public string ValueLabel { get; set; }
        public SKColor Color { get; set; }
    }
}