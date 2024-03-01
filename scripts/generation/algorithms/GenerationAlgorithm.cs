using System.Collections;

namespace ToolShed.MazeGeneration
{
    public class GenerationAlgorithm
    {
        public virtual void Step()
        {
            
        }

        public virtual void Run()
        {
            
        }

        public virtual void Animate()
        {
            
        }

        protected virtual IEnumerator StepAnimate()
        {
            yield break;
            Step();
        }
    }
}