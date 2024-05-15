import { useContext, useState } from "react";
import { AppContext } from "../Auth/AppProvider";
import { useSearchParams } from "react-router-dom";
import { Loading } from "../General/Loading";
import { Col, Grid, Input, InputGroup, Message, Panel, Row, Text } from "rsuite";
import { Unauthorized } from "../General/Unauthorized";
import { MdOutlineSearch } from "react-icons/md";
import { MyWeatherIcon } from "./WeatherCondition";

export const HistoryWeather = () => {
  const { store } = useContext(AppContext);
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [searchParams, setSearchParams] = useSearchParams();
  const [city, setCity] = useState("");
  const [serachWeather, setSearchWeather] = useState(true);

  useEffect(() => {
    if (serachWeather) {
      setSearchWeather(false);
      let cityFromParam = searchParams.get("cityName") ?? "Liberec";
      setCity(cityFromParam);
      getForecastWeather(cityFromParam);
    }
  }, [serachWeather]);

  const getHistoryWeather = (cityName) => {};

  if (loading) {
    return <Loading />;
  }
  if (error) {
    return <Message type="error" description={error} />;
  }
  if (!store.loggedIn) {
    return <Unauthorized />;
  }
  return (
    <Row>
      <Col xs={24} sm={24} md={10} lg={7}>
        <h3>Město</h3>
        <InputGroup>
          <Input value={city} onChange={(e) => setCity(e)} />
          <InputGroup.Button onClick={setSearchCity}>
            <MdOutlineSearch />
          </InputGroup.Button>
        </InputGroup>
      </Col>
      <Col xs={24} sm={24} md={14} lg={17}>
        <h3 style={{ textAlign: "center" }}>Počasí v {city}</h3>
        <Grid fluid>
          <Row gutter={5}>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel bordered header={<Text color="yellow">Název města</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel
                style={{ height: "142px" }}
                bordered
                header={<Text color="yellow">Zeměpisná šířka</Text>}
              >
                <Text style={{ fontSize: "25px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel
                bordered
                header={<Text color="yellow">Zeměpisná délka</Text>}
                style={{ height: "142px" }}
              >
                <Text style={{ fontSize: "25px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel bordered header={<Text color="yellow">Počasí</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                  {/*<MyWeatherIcon
                    weatherCondition={}
                    size={43}
  />*/}
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={8} xxl={8}>
              <Panel bordered header={<Text color="yellow">Teplota</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={8} xxl={8}>
              <Panel
                bordered
                header={<Text color="yellow">Pocitová teplota</Text>}
              >
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={8} xxl={8}>
              <Panel bordered header={<Text color="yellow">Oblačnost</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel bordered header={<Text color="yellow">Tlak</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel bordered header={<Text color="yellow">Vlhkost</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
              <Panel
                bordered
                header={<Text color="yellow">Rychlost větru</Text>}
              >
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={9} xl={6} xxl={6}>
              <Panel bordered header={<Text color="yellow">Směr větru</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={12} xl={12} xxl={12}>
              <Panel
                bordered
                header={<Text color="yellow">Východ slunce</Text>}
              >
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
            <Col xs={24} sm={12} md={12} lg={12} xl={12} xxl={12}>
              <Panel bordered header={<Text color="yellow">Západ Slunce</Text>}>
                <Text style={{ fontSize: "40px", textAlign: "center" }}>
                </Text>
              </Panel>
            </Col>
          </Row>
        </Grid>
      </Col>
    </Row>
  )
};
