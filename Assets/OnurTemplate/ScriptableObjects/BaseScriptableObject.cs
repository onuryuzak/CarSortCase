using UnityEngine;

namespace Onur.Template
{
    abstract public class BaseScriptableObject : ScriptableObject
    {
        virtual public void load()
        {
            // Override at the subclass.
        }

        virtual public void save()
        {
            // Override at the subclass.
        }

        virtual public void reset()
        {
            // Override at the subclass.
        }
    }
}
