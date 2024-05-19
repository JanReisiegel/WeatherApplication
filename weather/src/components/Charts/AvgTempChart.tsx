import React, { useMemo } from "react";
import { AxisOptions, Chart } from "react-charts";
type AvgTemps = {
  date: Date;
  stars: number;
};
type Series = {
  label: string;
  data: AvgTemps[];
};
export const AvgTempChart = ({ weather }) => {
  const data: Series[] = [
    {
      label: "Průměrná denní teplota",
      data: weather.map((day) => ({
        date: new Date(day.date),
        stars: day.day.avgTemp,
      })),
    },
  ];
  const primaryAxis: AxisOptions<AvgTemps> = useMemo(
    (): AxisOptions<AvgTemps> => ({
      getValue: (datum: AvgTemps) => datum.date,
    }),
    []
  );
  const secondaryAxes = useMemo(
    (): AxisOptions<AvgTemps>[] => [
      {
        getValue: (datum: AvgTemps) => datum.stars,
      },
    ],
    []
  );
  return (
    <Chart
      options={{
        data: data,
        primaryAxis: primaryAxis,
        secondaryAxis: secondaryAxes,
      }}
    />
  );
};
