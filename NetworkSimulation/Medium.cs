﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkSimulation
{
    public class Medium
    {
        public Medium()
        {
            Connectors = new List<Connector>();
        }

        public bool SharedMedium { get; set; }
        
        public List<Connector> Connectors { get; set; }

        public void Cycle()
        {
            
            foreach(Connector connSrc in Connectors)
            {
                connSrc.OutboundBytes = connSrc.ConnectedObject.GetData();
                byte[] bOutboundBytes = connSrc.OutboundBytes;
                foreach(Connector connDest in Connectors)
                {
                    if(connSrc.Id != connDest.Id || SharedMedium)
                    {
                        connDest.ReceiveBytes(bOutboundBytes);
                    }
                }
            }
            foreach(Connector conn in Connectors)
            {
                conn.ConnectedObject.ReceiveData(conn.InboundBytes);
                conn.InboundBytes = null;
            }
        }

        public class Connector
        {
            public Connector(IConnection connectedObject) : this()
            {
                ConnectedObject = connectedObject;
            }
            public Connector()
            {
                Id = Guid.NewGuid();
            }
            public Guid Id { get; set; }
            public IConnection ConnectedObject { get; set; }
            public void SendBytes(byte[] bSend)
            {
                OutboundBytes = (byte[])bSend.Clone();
            }
            public void ReceiveBytes(byte[] bReceive)
            {
                if (bReceive != null)
                {
                    if (InboundBytes == null) InboundBytes = new byte[0];
                    if (InboundBytes.Length < bReceive.Length)
                    {
                        byte[] bNewLength = (byte[])bReceive.Clone();
                        Array.Copy(InboundBytes, bNewLength, InboundBytes.Length);
                        InboundBytes = bNewLength;
                    }

                    for (int i = 0; i < bReceive.Length; i++)
                    {
                        InboundBytes[i] = (byte)(bReceive[i] | InboundBytes[i]);
                    }
                }
            }
        
            


            public byte[] OutboundBytes { get; set; }
            public byte[] InboundBytes { get; set; }
        }
    }
}
