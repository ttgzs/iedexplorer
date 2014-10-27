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

    public enum CommandCtrlModel
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
        public CommandCtrlModel CommandFlowFlag;
        public object ctlVal;
        public OrCat orCat;
        public string orIdent;
        public DateTime T;
        public bool Test;
        public bool interlockCheck;
        public bool synchroCheck;
        public string Address;
    }

    public enum OrCat
    {
        /** Not supported - should not be used */
        NOT_SUPPORTED = 0,
        /** Control operation issued from an operator using a client located at bay level */
        BAY_CONTROL = 1,
        /** Control operation issued from an operator using a client located at station level */
        STATION_CONTROL = 2,
        /** Control operation from a remote operator outside the substation (for example network control center) */
        REMOTE_CONTROL = 3,
        /** Control operation issued from an automatic function at bay level */
        AUTOMATIC_BAY = 4,
        /** Control operation issued from an automatic function at station level */
        AUTOMATIC_STATION = 5,
        /** Control operation issued from a automatic function outside of the substation */
        AUTOMATIC_REMOTE = 6,
        /** Control operation issued from a maintenance/service tool */
        MAINTENANCE = 7,
        /** Status change occurred without control action (for example external trip of a circuit breaker or failure inside the breaker) */
        PROCESS = 8
    }
}
