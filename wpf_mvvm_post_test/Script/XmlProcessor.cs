using System;
using System.Xml;
using GalaSoft.MvvmLight.Messaging;

namespace wpf_mvvm_post_test
{
    public class XmlProcessor
    {
        public void Reciever(Responder response)
        {
            switch (response._response)
            {
                case Constants.REC_XML_XMLPROC:
                    Messenger.Default.Register<XmlLoadFormat>(this, Dynamics);
                    break;
            }

            Messenger.Default.Send<NotificationMessage>(new NotificationMessage("XML RECIEVE "+response._response));
        }

        void Dynamics(XmlLoadFormat xml)
        {
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xml._path);
                XmlNode _node = xmldoc.SelectSingleNode("configuration");
                XmlNode _nodeSet = _node.SelectSingleNode(xml._node);
                switch (xml._mode)
                {
                    case 0: // LOAD SINGLE VALUE
                        foreach (XmlNode d in _nodeSet)
                        {
                            if (d.Attributes["key"].Value == xml._singleKey)
                            {
                                xml._callback(d.Attributes["value"].Value);
                                Messenger.Default.Send<NotificationMessage>(new NotificationMessage("LOAD :" + d.Attributes["key"].Value + " VAL: " + d.Attributes["value"].Value));
                            }
                        }
                        break;

                    case 1: // SAVE MULTIPLE
                        int length = xml._multiKey.Length;
                        foreach (XmlNode d in _nodeSet)
                        {
                            for (int i = 0; i < length; i++)
                            {
                                if (d.Attributes["key"].Value == xml._multiKey[i])
                                {
                                    d.Attributes["value"].Value = xml._storeVal[i];
                                    Messenger.Default.Send<NotificationMessage>(new NotificationMessage("SAVE :" + d.Attributes["key"].Value + " VAL: " + d.Attributes["value"].Value));
                                }
                            }
                        }
                        xmldoc.Save(xml._path);
                        xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                        break;
                }

            } catch(Exception e)
            {
                Console.WriteLine(e + " XML NOT FOUND: " + AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName + ".config");
            }
        }
    }
}
