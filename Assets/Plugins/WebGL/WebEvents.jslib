mergeInto(LibraryManager.library, {
	
	LogGameEvent: function (eventJSON) {
		ReactUnityWebGL.LogEvent(Pointer_stringify(eventJSON));
	},
	
	GameOver: function() {
		ReactUnityWebGL.GameOver();
	}

});