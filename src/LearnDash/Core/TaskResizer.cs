using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public interface IResizable
    {
        int Counter { get; }
        int Width { set; }
        int Height { set; }
        int 
    }

    public class TaskResizer
    {
        private const int MaxFont = 24;
        private const int MinFont = 9;
        private const int FontScale = 5;

        private const int MaxSize = 400;
        private const int MinSize = 50;

        public List<IResizable> Resize(List<IResizable> elements)
        {
            if(elements == null) throw new ArgumentNullException("learningTasks task cant be null");

            if(elements.Count <= 0)
            {
                return elements;
            }

            var difference = this.CalculateDifference(elements);

            throw new NotImplementedException();
        }

        public int CalculateDifference(List<IResizable> elements)
        {
            if(elements == null) throw new ArgumentNullException();
            if(elements.Count <= 0) throw new ArgumentOutOfRangeException("you cant calculate difference on empty list"); 
            if(elements.Any(x => x.Counter < 0)) throw new ArgumentOutOfRangeException("elements cant have counter less than 0");

            if(elements.Count == 1)
            {
                return elements.First().Counter;
            }

            var max = elements.Max(x => x.Counter);
            var min = elements.Min(x => x.Counter);

            return max - min;
        }

        public int CalculateHeightWidth(int factor)
        {
            var value = (int) (MaxSize/Math.Pow(2, factor));
            return  value < MinSize ? value : MinSize;
        }

        //refactor:  this could be reworked so FontScale is determineg automaticlly based on scale 
        public int GetNewFontSize(int factor)
        {   var value = MaxFont - (factor * FontScale);
            return value < MinFont ? MinFont : value;
        }
    }
}
