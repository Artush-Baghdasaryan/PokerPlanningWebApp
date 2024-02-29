using MongoDB.Bson;
using PokerPlanningWebApi.Intefaces;
using PokerPlanningWebApi.Models;
using PokerPlanningWebApi.Rpositories;

namespace PokerPlanningWebApi.Services;

public class RoomService : IRoomService
{
    private readonly RoomRepository _roomRepository;
    private readonly GuestRepository _guestRepository;

    public RoomService(RoomRepository roomRepository, GuestRepository guestRepository)
    {
        _roomRepository = roomRepository;
        _guestRepository = guestRepository;
    }

    public async Task<List<Room>> GetAll()
    {
        var rooms = await _roomRepository.GetAll();
        return rooms;
    }

    public async Task<Room> GetById(string roomId)
    {
        var room = await _roomRepository.GetById(roomId);
        if (room == null)
        {
            throw new Exception("Room was not found!");
        }

        return room;
    }

    public async Task<string> CreateRoom(string adminId, string roomName)
    {
        var admin = await _guestRepository.GetById(adminId);
        if (admin == null) {throw new Exception("Admin was not found!");}
        var room = new Room
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = roomName,
            Admin = admin,
            Guests = new List<Guest>()
        };
        room.Guests.Add(admin);
        await _roomRepository.AddEntity(room);
        return room.Id;
    }

    public async Task DeleteRoom(string roomId)
    {
        if (!await _roomRepository.DoesExist(roomId))
        {
            throw new Exception("Room was not found!");
        }

        await _roomRepository.Delete(roomId);
    }

    public async Task<string> AddGuest(string roomId, string guestId)
    {
        var room = await _roomRepository.GetById(roomId);
        var guest = await _guestRepository.GetById(guestId);
        if (room == null || guest == null) throw new Exception("Something went wrong, try again");
        await _roomRepository.AddGuestToRoom(room, guest);
        return guestId;
    }

    public async Task RemoveGuest(string roomId, string guestId)
    {
        var room = await _roomRepository.GetById(roomId);
        var guest = await _guestRepository.GetById(guestId);
        if (room == null || guest == null) throw new Exception("Something went wrong, try again");
        await _roomRepository.RemoveGuestFromRoom(room, guest);
        await _guestRepository.Delete(guestId);
    }   
    
}