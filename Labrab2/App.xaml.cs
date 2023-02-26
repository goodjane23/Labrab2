using System.IO;
using System.Windows;
using Labrab2.Services.Hotel;

namespace Labrab2;

public partial class App : Application
{
    public static readonly IHotelService HotelService = new HotelService();

	public App()
	{
		Directory.CreateDirectory("Data");
	}
}
