using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ZimmerFrei.Data
{


    public class ApartmentData
    {
        public ApartmentData(String id, String name, String description, String capacity, String stars, String address, String email, String phone, String phone2, String rating, String lat, String lng, String price, String cover_photo, String owner_id, String type_id, String city_id)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Capacity = capacity;
            this.Stars = stars;
            this.Address = address;
            this.Email = email;
            this.Phone = phone;
            this.Phone2 = phone2;
            this.Rating = rating;
            this.Lat = lat;
            this.Lng = lng;
            this.Price = price;
            this.Cover_photo = cover_photo;
            this.Owner_id = owner_id;
            this.Type_id = type_id;
            this.City_id = city_id;


        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Capacity { get; private set; }
        public string Stars { get; private set; }
        public string Address { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Phone2 { get; private set; }
        public string Rating { get; private set; }
        public string Lat { get; private set; }
        public string Lng { get; private set; }
        public string Price { get; private set; }
        public string Cover_photo { get; private set; }
        public string Owner_id { get; private set; }
        public string Type_id { get; private set; }
        public string City_id { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }



    public sealed class DataSource
    {
        private static DataSource _dataSource = new DataSource();

        private ObservableCollection<ApartmentData> _apartments = new ObservableCollection<ApartmentData>();

        public ObservableCollection<ApartmentData> Apartments
        {
            get { return this._apartments; }
        }

        public static async Task<IEnumerable<ApartmentData>> GetApartmentsAsync()
        {   
            await _dataSource.GetDataAsync();

            return _dataSource.Apartments;
        }

        public static async Task<ApartmentData> GetApartmentAsync(string id)
        {
            await _dataSource.GetDataAsync();
            var matches = _dataSource.Apartments.Where((apartment) => apartment.Id.Equals(id));
            if (matches.Count() == 1) return matches.First();
            return null;
        }



        private async Task GetDataAsync()
        {
            if (this._apartments.Count != 0)
                return;

            Uri dataUri = new Uri("ms-appx:///DataModel/Apartments.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["apartments"].GetArray();

            foreach (JsonValue apartmentValue in jsonArray)
            {
                JsonObject apartmentObject = apartmentValue.GetObject();
                ApartmentData apartment = new ApartmentData(apartmentObject["Id"].GetString(),
                                                            apartmentObject["Name"].GetString(),
                                                            apartmentObject["Description"].GetString(),
                                                            apartmentObject["Capacity"].GetString(),
                                                            apartmentObject["Stars"].GetString(),
                                                            apartmentObject["Address"].GetString(),
                                                            apartmentObject["Email"].GetString(),
                                                            apartmentObject["Phone"].GetString(),
                                                            apartmentObject["Phone2"].GetString(),
                                                            apartmentObject["Rating"].GetString(),
                                                            apartmentObject["Lat"].GetString(),
                                                            apartmentObject["Lng"].GetString(),
                                                            apartmentObject["Price"].GetString(),
                                                            apartmentObject["Cover_photo"].GetString(),
                                                            apartmentObject["Owner_id"].GetString(),
                                                            apartmentObject["Type_id"].GetString(),
                                                            apartmentObject["City_id"].GetString());

                this.Apartments.Add(apartment);
            }
        }
    }
}
