using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aerospace.Model;

namespace Aerospace.ViewModels;

internal class JourneyConverter : JsonConverter<SpacecraftJourney>
{
    #region Constants and Fields

    private readonly Model.Model model;

    #endregion

    #region Constructors and Destructors

    internal JourneyConverter(Model.Model model)
    {
        this.model = model;
    }

    #endregion

    #region Public Methods

    public override SpacecraftJourney Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var journey = new SpacecraftJourney(model);
        journey.Route.Clear();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return journey;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string? propertyName = reader.GetString();

            switch (propertyName)
            {
                case "name":
                {
                    reader.Read();
                    string? name = reader.GetString();

                    if (name is null)
                    {
                        throw new JsonException();
                    }

                    journey.Name = name;

                    break;
                }
                case "spacecraft":
                {
                    reader.Read();
                    string? spacecraftName = reader.GetString();
                    Spacecraft spacecraft =
                        model.Spacecrafts.FirstOrDefault(spacecraft => spacecraft.Name == spacecraftName);

                    if (string.IsNullOrEmpty(spacecraft.Name))
                    {
                        throw new JsonException();
                    }

                    journey.Spacecraft = spacecraft;

                    break;
                }
                case "numPassengers":
                {
                    reader.Read();
                    journey.NumPassengers = reader.GetInt32();

                    break;
                }
                case "route":
                {
                    reader.Read();

                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        throw new JsonException();
                    }

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                        {
                            break;
                        }

                        if (reader.TokenType != JsonTokenType.String)
                        {
                            throw new JsonException();
                        }

                        string? planetName = reader.GetString();

                        Planet planet = model.Planets.FirstOrDefault(planet => planet.Name == planetName);

                        if (string.IsNullOrEmpty(planet.Name))
                        {
                            throw new JsonException();
                        }

                        journey.Route.Add(planet);
                    }

                    break;
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, SpacecraftJourney value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("name", value.Name);

        writer.WriteString("spacecraft", value.Spacecraft.Name);

        writer.WriteNumber("numPassengers", value.NumPassengers);

        writer.WriteStartArray("route");

        foreach (Planet planet in value.Route)
        {
            writer.WriteStringValue(planet.Name);
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    #endregion
}