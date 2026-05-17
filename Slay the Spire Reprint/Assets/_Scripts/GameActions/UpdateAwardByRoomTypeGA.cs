using UnityEngine;

public class UpdateAwardByRoomTypeGA : GameAction
{
    public RoomType roomType;

    public UpdateAwardByRoomTypeGA(RoomType roomType)
    {
        this.roomType = roomType;
    }
}
