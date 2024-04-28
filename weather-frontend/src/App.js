import "./App.css";
import "rsuite/dist/rsuite.min.css";
import { AppProvider } from "./components/Auth/AppProvider";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Navigation from "./components/Layout/Navigation";
import Unauthorized from "./components/General/Unauthorized";
import { Col, Row } from "rsuite";

function App() {
  return (
    <AppProvider>
      <BrowserRouter basename="/">
        <Row>
          <Col sx={24} sm={24} md={3} lg={2}></Col>
          <Col sx={24} sm={24} md={18} lg={20}>
            <Navigation />
            <Routes>
              <Route path="/" element={<Unauthorized />} />
            </Routes>
          </Col>
          <Col sx={24} sm={24} md={3} lg={2}></Col>
        </Row>
      </BrowserRouter>
    </AppProvider>
  );
}

export default App;
