using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IEDExplorer
{
    public enum CommandType
    {
        SingleCommand,
        DoubleCommand,
        IntegerCommand,
        EnumCommand,
        BinaryStepCommand,
        AnalogueSetpoint,
        AnalogueByBinary,
    }

    public enum CommandFlow
    {
        State_Only = 0,
        Direct_Control_With_Normal_Security,
        Select_Before_Operate_With_Normal_Security,
        Direct_Control_With_Enhanced_Security,
        Select_Before_Operate_With_Enhanced_Security,
        Unknown,
    }

    public class CommandParams
    {
        public CommandType CommType;
        public scsm_MMS_TypeEnum DataType;
        public CommandFlow CommandFlowFlag;
        public object ctlVal;
        public long orCat;
        public string orIdent;
        public DateTime T;
        public bool Test;
        public bool interlockCheck;
        public bool synchroCheck;
        public string Address;
    }
}
