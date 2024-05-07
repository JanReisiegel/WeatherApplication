import { useContext, useEffect, useState } from "react";
import { AppContext } from "../Auth/AppProvider";
import axios from "axios";
import { LocationApi } from "../../configuration/API";
import { Loading } from "../General/Loading";
import Unauthorized from "../General/Unauthorized";
import {
  Button,
  ButtonGroup,
  Col,
  FlexboxGrid,
  IconButton,
  Message,
  Panel,
  Row,
} from "rsuite";
import FlexboxGridItem from "rsuite/esm/FlexboxGrid/FlexboxGridItem";
import { Link } from "react-router-dom";
import { MdAddCircleOutline, MdOutlineModeEdit } from "react-icons/md";
import { IoTrashOutline } from "react-icons/io5";

export const SavedLocations = () => {
  const { store } = useContext(AppContext);
  const [locations, setLocations] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    getSavedlocations();
  }, []);
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
  const deleteLocation = (customName) => {
    axios
      .delete(LocationApi.basic + "?customName=" + customName, {
        headers: {
          "Content-Type": "application/json",
          "Access-Control-Allow-Origin": "http://localhost:3000",
          userToken: store.token ? store.token : "",
        },
      })
      .then((response) => {
        console.log(response);
        if (response.status === 200) {
          getSavedlocations();
        }
      })
      .catch((error) => {
        console.error(error);
      });
  };

  if (loading) {
    return <Loading />;
  }
  if (error || !store.loggedIn) {
    return <Unauthorized />;
  }
  return (
    <Row>
      <Col xs={24} sm={24} md={24} lg={24} xl={24} xxl={24}>
        <Row>
          <Col xs={24} sm={24} md={16} lg={16} xl={16} xxl={16}>
            <h1 style={{ textAlign: "center" }}>Uložená místa</h1>
          </Col>
          <Col xs={24} sm={24} md={8} lg={8} xl={8} xxl={8}>
            <IconButton
              as={Link}
              to={"/add-location"}
              color="green"
              appearance="primary"
              icon={<MdAddCircleOutline size={36} />}
            ></IconButton>
          </Col>
        </Row>

        <FlexboxGrid justify="space-around">
          {locations.length > 0 ? (
            locations.map((location) => {
              return (
                <FlexboxGrid.Item colspan={6}>
                  <Panel
                    bordered
                    header={location.customName + " - " + location.cityName}
                  >
                    <dl>
                      <dt>Latitude</dt>
                      <dd>{location.latitude}</dd>
                      <dt>Longitude</dt>
                      <dd>{location.longitude}</dd>
                    </dl>
                    <ButtonGroup justified>
                      <Button
                        color="violet"
                        appearance="primary"
                        as={Link}
                        to={"/actual-weather?cityName=" + location.cityName}
                      >
                        Teď
                      </Button>
                      <Button
                        color="blue"
                        appearance="primary"
                        as={Link}
                        to={"/actual-weather?cityName=" + location.cityName}
                      >
                        Potom
                      </Button>
                      <IconButton
                        color="red"
                        onClick={() => deleteLocation(location.customName)}
                        appearance="primary"
                        icon={<IoTrashOutline size={16} />}
                      />
                    </ButtonGroup>
                  </Panel>
                </FlexboxGrid.Item>
              );
            })
          ) : (
            <FlexboxGrid.Item colspan={24}>
              <Message
                showIcon
                type="info"
                header="Nemáte uložená místa"
                style={{ textAlign: "center" }}
              >
                <p>Zatím nemáte uložená žádná místa.</p>
              </Message>
            </FlexboxGrid.Item>
          )}
        </FlexboxGrid>
      </Col>
    </Row>
  );
};
