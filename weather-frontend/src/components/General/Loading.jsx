import React from "react";
import { GuardSpinner } from "react-spinners-kit";

const Loading = () => {
  return (
    <div style={{ marginLeft: "40vw", marginTop: "40vh" }}>
      <GuardSpinner size={50} color="#686769" loading={true} />
      <br />
      <p>Načítání ...</p>
    </div>
  );
};
