import React from "react";
import { GuardSpinner } from "react-spinners-kit";

export const Loading = () => {
  return (
    <div style={{ marginLeft: "40vw", marginTop: "40vh" }}>
      <GuardSpinner
        size={70}
        frontColor="#00aaff"
        backColor="#ffaa00"
        loading={true}
      />
      <br />
      <p>Načítání ...</p>
    </div>
  );
};
