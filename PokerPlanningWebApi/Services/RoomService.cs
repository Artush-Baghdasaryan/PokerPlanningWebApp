using MongoDB.Bson;
using MongoDB.Driver.Linq;
using PokerPlanningWebApi.Intefaces;
using PokerPlanningWebApi.Models;
using PokerPlanningWebApi.Rpositories;

namespace PokerPlanningWebApi.Services;

public class RoomService : IRoomService
{
    private readonly RoomRepository _roomRepository;
    private readonly IGuestService _guestService;
    
    public RoomService(RoomRepository roomRepository, IGuestService guestService)
    {
        _roomRepository = roomRepository;
        _guestService = guestService;
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

    public async Task<IList<Guest>?> GetGuests(string roomId)
    {
        var room = await _roomRepository.GetById(roomId);
        if (room == null) throw new Exception("Room was not found");

        var guests = new List<Guest>();
        foreach (var guestId in room.GuestsIds)
        {
            guests.Add(await _guestService.GetById(guestId));
        }
        return guests;
    }

    public async Task<Room> CreateRoom(string roomName)
    {
        var room = new Room
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Name = roomName,
            Admin = null,
            RevealScore = null,
            GuestsIds = new List<string>()
        };
        await _roomRepository.AddEntity(room);
        return room;
    }

    public async Task DeleteRoom(string roomId)
    {
        if (!await _roomRepository.DoesExist(roomId))
        {
            throw new Exception("Room was not found!");
        }

        await _roomRepository.Delete(roomId);
    }

    public async Task<Guest> AddGuest(string roomId, string? connectionId = null)
    {
        var room = await _roomRepository.GetById(roomId);
        if (room == null) throw new Exception("Something went wrong, try again");

        var indexes = (await GetGuests(roomId)).Select(g => g.Index);
        var allowedIndexes = Enumerable.Range(1, 8).Where(i => indexes != null && !indexes.Contains(i));
        var rnd = new Random();
        var newIndex = allowedIndexes.ToList()[rnd.Next(1, allowedIndexes.Count())];
        var guest = await _guestService.CreateGuest(newIndex, connectionId);
        if (indexes?.Count() == 0)
        {
            room.Admin = guest;
            await _roomRepository.UpdateEntity(roomId, room);
        }
        
        await _roomRepository.AddGuestToRoom(room, guest);
        return guest;
    }

    public async Task RemoveGuest(string roomId, string guestId)
    {
        var room = await _roomRepository.GetById(roomId);
        var guest = await _guestService.GetById(guestId);
        if (room == null || guest == null) throw new Exception("Something went wrong, try again");
        if (room.Admin?.Id == guestId)
        {
            await _roomRepository.Delete(roomId);
        }
        else
        {
            await _roomRepository.RemoveGuestFromRoom(room, guest);
        }
        
        await _guestService.RemoveGuest(guestId);
    }

    public async Task<int?> Reveal(string roomId)
    {
        var room = await _roomRepository.GetById(roomId);
        if (room == null) throw new Exception("Room was not found");

        
        var revealScore = (await GetGuests(roomId)).Sum(g => g.Score);
        room.RevealScore = revealScore;
        await _roomRepository.UpdateEntity(roomId, room);

        return revealScore;
    }

    public async Task ResetVoting(string roomId)
    {
        var room = await _roomRepository.GetById(roomId);
        if (room == null) throw new Exception("Room was not found");

        room.RevealScore = null;
        await _roomRepository.UpdateEntity(roomId, room);
        foreach (var guestId in room.GuestsIds)
        {
            await _guestService.UpdateScore(guestId, null);
        }
    }
}













