﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Czeum.DTO.Lobby {
	public class LobbyData {
		public int LobbyId { get; set; }
		public string Host { get; set; }
		public string Guest { get; set; }
		public int BoardHeight { get; set; }
		public int BoardWidth { get; set; }
		public LobbyAccess Access { get; set; }
		public List<string> InvitedPlayers { get; set; }
	}
}
