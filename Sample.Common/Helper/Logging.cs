using MethodBoundaryAspect.Fody.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sample.Common.Helper.ExtentReportsHelper;


namespace Sample.Common.Helper
{
    public sealed class Logging : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            CreateStepNode(args.Method.Name);
            var parameters = args.Method.GetParameters();
            if (args.Arguments.Count() > 0)
            {
                GetLastNode().Info("Method arguments");
                Dictionary <string, object> argumentDict = new Dictionary<string, object>();
                for (int i = 0; i < parameters.Count(); i++)
                    argumentDict[parameters[i].Name] = args.Arguments[i];
                GetLastNode().Info(argumentDict.ConvertObjectToJson().MarkupJsonString());
            }
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            EndStepNode(GetLastNode(), args.Method.Name);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            GetLastNode().Error(args.Exception.Message + " " + args.Exception.StackTrace);
            EndStepNode(GetLastNode(), args.Method.Name);
        }
    }
}
