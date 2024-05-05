import React, { useContext, useState } from "react";
import { AppContext } from "../Auth/AppProvider";
import { WeatherApi } from "../../configuration/API";
import { useSearchParams } from "react-router-dom";
import axios from "axios";
import { GuardSpinner } from "react-spinners-kit";
import { Loading } from "../General/Loading";
import { Button, Message } from "rsuite";

const ActualWeather = () => {
  const { store } = useContext(AppContext);
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [searchParams, setSearchParams] = useSearchParams();
  const [city, setCity] = useState("");

  const loadWeather = () => {
    axios
      .get(WeatherApi.current + "?cityName" + searchParams.get("cityName"), {
        headers: {
          "Content-Type": "application/json",
          "Access-Control-Allow-Origin": "http://localhost:3000",
          userToken: store.token ? store.token : "",
        },
      })
      .then((response) => {
        console.log(response);
        if (response.status === 200) {
          setWeather(response.data);
        }
        searchParams.set("cityName", city);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  if (loading) {
    return <Loading />;
  }
  if (error) {
    return <Message type="error" description={error} />;
  }
  return (
    <div>
      <h1>Aktuální počasí</h1>
      <input
        type="text"
        value={city}
        onChange={(e) => setCity(e.target.value)}
      />
      <Button onClick={loadWeather}>Načíst</Button>
    </div>
  );
};

export default ActualWeather;
