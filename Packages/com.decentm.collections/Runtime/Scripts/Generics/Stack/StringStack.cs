using UnityEngine;

namespace DecentM.Collections.Generics
{
    /// <summary>
    /// A stack of strings
    /// </summary>
    [
        AddComponentMenu("DecentM/Collections/Generics/Stack/StringStack"),
        DisallowMultipleComponent,
    ]
    public sealed class StringStack : Stack
    {
        public bool Contains(string item)
        {
            return this.Contains((object)item);
        }

        public bool Push(string item)
        {
            return this.Push((object)item);
        }

        public string PeekString()
        {
            return (string)this.Peek();
        }

        public string PopString()
        {
            return (string)this.Pop();
        }
    }
}
