using System;

namespace wpf_mvvm_post_test
{
    class Constants
    {
        public const string XML_NODE_PRT = "configuration";
        public const string XML_NODE_CHD = "appSettings";

        public const string XML_CONF_ATTR_API = "API";
        public const string XML_CONF_ATTR_TKN = "TOKEN";
        public const string XML_CONF_ATTR_INTR = "INTERVAL";
        public const string XML_CONF_ATTR_DB = "DB NAME";

        public const string CMD_XML_LOAD = "LOAD_X";
        public const string CMD_XML_LOAD_SINGLE = "LOAD_X_SINGLE";
        public const string CMD_DATA_LOAD = "LOAD";
        public const string CMD_OPEN_CONFIG = "OPEN_CONFIG";
        public const string CMD_CLOSE_CONFIG = "CLOSE_CONFIG";
        public const string CMD_SND_CONFIG = "SENDING_CONF";
        public const string CMD_GET_CONFIG = "GET_CONFIG";
        public const string CMD_RUN_UPDATE = "UPDATE_DATA";
        public const string CMD_RUN_DELETE = "DELETE_DATA";
        public const string CMD_RUN_CREATE = "CREATE_DATA";
        public const string CMD_SHOW_ERROR = "SHOW_ERROR";
        public const string TIMER_INIT = "TIMER_ON";

        public const string REC_XML_LOAD = "LOAD_ALL";
        public const string REC_DATA_LOAD = "LOAD_DATA";
        public const string REC_XML_XMLPROC = "INIT_XMLPROC";

        public const string SENT_CONF = "SEND_CONF";
        public const string GET_CONF = "GET_CONF";
    }
}
