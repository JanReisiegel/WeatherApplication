import "./App.css";
import "rsuite/dist/rsuite.min.css";
import { AppProvider } from "./components/Auth/AppProvider";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Navigation from "./components/Layout/Navigation";
import Unauthorized from "./components/General/Unauthorized";
import { Col, Row } from "rsuite";
import ActualWeather from "./components/Weather/ActualWeather";
import { SavedLocations } from "./components/Locations/SavedLocations";
import { AddLocation } from "./components/Locations/AddLocation";
import { ForecastWeather } from "./components/Weather/ForecastWeather";

function App() {
  return (
    <AppProvider>
      <BrowserRouter basename="/">
        <Row>
          <Col sx={24} sm={24} md={3} lg={2}></Col>
          <Col sx={24} sm={24} md={18} lg={20}>
            <Navigation />
            <Routes>
              <Route path="/" element={<ActualWeather />} />
              <Route path="/actual-weather" element={<ActualWeather />} />
              <Route path="/forecats" element={<ForecastWeather />} />
              <Route path="/saved-locations" element={<SavedLocations />} />
              <Route path="/add-location" element={<AddLocation />} />
            </Routes>
          </Col>
          <Col sx={24} sm={24} md={3} lg={2}></Col>
        </Row>
      </BrowserRouter>
    </AppProvider>
  );
}

export default App;
