public static class GameEventManager {
	
	public delegate void GameEvent(int roomNumber);
	
	public static event GameEvent StepIn;
	
	public static void TriggerStepIn( int roomNumber )
	{
		if (StepIn != null)
		{
			StepIn (roomNumber);
		}
	}

}