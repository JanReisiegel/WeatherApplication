import { useContext, useState } from "react";
import { AppContext } from "../Auth/AppProvider";
import axios from "axios";
import { LocationApi } from "../../configuration/API";
import { Loading } from "../General/Loading";
import Unauthorized from "../General/Unauthorized";
import { Button, ButtonGroup, FlexboxGrid, Panel } from "rsuite";
import FlexboxGridItem from "rsuite/esm/FlexboxGrid/FlexboxGridItem";
import { Link } from "react-router-dom";

const SavedLocations = () => {
  const { store } = useContext(AppContext);
  const [locations, setLocations] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const getSavedlocations = () => {
    axios
      .get(LocationApi.getAll, {
        headers: {
          "Content-Type": "application/json",
          "Access-Control-Allow-Origin": "http://localhost:3000",
          userToken: store.token ? store.token : "",
        },
      })
      .then((response) => {
        console.log(response);
        if (response.status === 200) {
          setLocations(response.data);
        }
      })
      .catch((error) => {
        console.error(error);
      });
  };

  if (loading) {
    return <Loading />;
  }
  if (error) {
    return <Unauthorized />;
  }
  return (
    <Row>
      <Col xs={24} sm={24} md={24} lg={24} xl={24} xxl={24}>
        <h1>Uložená místa</h1>
        <FlexboxGrid justify="space-between">
          {locations.map((location) => {
            return (
              <FlexboxGridItem colspan={4}>
                <Panel bordered header={location.customName}>
                  <dl>
                    <dt>Latitude</dt>
                    <dd>{location.latitude}</dd>
                    <dt>Longitude</dt>
                    <dd>{location.longitude}</dd>
                  </dl>
                  <ButtonGroup>
                    <Button
                      appearance="primary"
                      as={Link}
                      to={"/actual-weather?cityName=" + location.cityName}
                    >
                      Aktuálně
                    </Button>
                    <Button
                      appearance="primary"
                      as={Link}
                      to={"/actual-weather?cityName=" + location.cityName}
                    >
                      Předpověď
                    </Button>
                  </ButtonGroup>
                </Panel>
              </FlexboxGridItem>
            );
          })}
        </FlexboxGrid>
      </Col>
    </Row>
  );
};
