using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ZimmerFrei.Data
{
    [DataContract]
    public class Apartment
    {
        [DataMember(Name = "id")]
        public string Id { get; private set; }
        [DataMember(Name = "name")]
        public string Name { get; private set; }
        [DataMember(Name = "description")]
        public string Description { get; private set; }
        [DataMember(Name = "capacity")]
        public string Capacity { get; private set; }
        [DataMember(Name = "stars")]
        public string Stars { get; private set; }
        [DataMember(Name = "address")]
        public string Address { get; private set; }
        [DataMember(Name = "email")]
        public string Email { get; private set; }
        [DataMember(Name = "phone")]
        public string Phone { get; private set; }
        [DataMember(Name = "phone_2")]
        public string Phone2 { get; private set; }
        [DataMember(Name = "rating")]
        public string Rating { get; private set; }
        [DataMember(Name = "lat")]
        public string Lat { get; private set; }
        [DataMember(Name = "lng")]
        public string Lng { get; private set; }
        [DataMember(Name = "price")]
        public string Price { get; private set; }
        [DataMember(Name = "cover_photo")]
        public string CoverPhoto { get; private set; }
        [DataMember(Name = "owner_id")]
        public string OwnerId { get; private set; }
        [DataMember(Name = "type_id")]
        public string TypeId { get; private set; }
        [DataMember(Name = "city_id")]
        public string CityId { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    [DataContract]
    public class ApartmentsPayload
    {
        [DataMember(Name = "apartments")]
        public List<Apartment> Apartments { get; private set; }
    }

    public sealed class DataSource
    {
        private static DataSource _dataSource = new DataSource();

        private ObservableCollection<Apartment> _apartments = new ObservableCollection<Apartment>();

        public ObservableCollection<Apartment> Apartments
        {
            get { return this._apartments; }
        }

        public static async Task<IEnumerable<Apartment>> GetApartmentsAsync()
        {   
            await _dataSource.GetDataAsync();

            return _dataSource.Apartments;
        }

        public static async Task<Apartment> GetApartmentAsync(string id)
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
            var serializer = new DataContractJsonSerializer(typeof(ApartmentsPayload));
            using (var stream = await file.OpenStreamForReadAsync())
            {
                var payload = (ApartmentsPayload)serializer.ReadObject(stream);
                foreach (var apartment in payload.Apartments)
                    Apartments.Add(apartment);
            }
        }
    }
}
