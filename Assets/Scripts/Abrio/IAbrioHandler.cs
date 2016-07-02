using System;

namespace Abrio
{
	public interface IAbrioHandler
	{
		void onConnected();
		void onBasicEvent(BasicEvent abrioEvent);
	}
}

