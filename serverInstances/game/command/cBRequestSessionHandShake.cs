﻿using SuperSocket.SocketBase.Command;

using Libraries.enums;
using Libraries.player;
using Libraries.database;
using Libraries.logger;
using Libraries.packages.custom;

using Libraries.helpers.package;


namespace Game.Command
{

    public class BRequestSessionHandShake : CommandBase<Session, Package>
    {

        /// <summary>
        /// Executes the command and sends response.
        /// </summary>
        /// <param name="s">The session.</param>
        /// <param name="p">The package info.</param>
        public override void ExecuteCommand(Session s, Package p)
        {

            PacketBRequestSessionHandShake Request = new PacketBRequestSessionHandShake(p.Content);

            Logger.Debug($"{p.Key}::ExecuteCommand - Execute command: {Request}");

            if (!s.IsAuthenticated)
            {

                Player ObjPlayer = Database.Players.Get(Request.PlayerName, Request.Password);

                if (ObjPlayer != null)
                {

                    s.SetPlayer(ObjPlayer);

                }

            }

            PacketBResponseSessionHandShake ResponseContent = new PacketBResponseSessionHandShake(1);

            Logger.Debug($"{p.Key}::ExecuteCommand - Execute command: {ResponseContent}");

            byte[] Response = ResponseContent.ToByteArray();

            Package Package = new Package(p.HeaderXuid, p.HeaderField20, p.HeaderServiceId, p.HeaderField22, PacketTypes.BResponseSessionHandShake, p.HeaderRequestId, Response);

            byte[] ToSend = Package.ToByteArray();

            s.Send(ToSend, 0, ToSend.Length);

        }

    }

}
