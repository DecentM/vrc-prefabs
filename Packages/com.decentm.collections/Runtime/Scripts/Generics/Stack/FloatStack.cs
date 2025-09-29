using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A stack of floats
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Stack/FloatStack"),
        DisallowMultipleComponent,
    ]
    public sealed class FloatStack : Stack
    {
        public bool Contains(float item)
        {
            return this.Contains((object)item);
        }

        public bool Push(float item)
        {
            return this.Push((object)item);
        }

        public float PeekFloat()
        {
            return (float)this.Peek();
        }

        public float PopFloat()
        {
            return (float)this.Pop();
        }
    }
}
