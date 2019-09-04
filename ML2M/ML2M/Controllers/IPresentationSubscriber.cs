using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ML2M.Controllers
{
    public interface IPresentationSubscriber
    {
        void HandlePresentationEvent(PresentationEvents presentationEvent, params object[] arg);
    }
}
