import {
  IoCloudyOutline,
  IoRainyOutline,
  IoSnowOutline,
  IoSunnyOutline,
  IoThunderstormOutline,
} from "react-icons/io5";
import { BsCloudDrizzle, BsCloudFog2, BsCloudHaze2 } from "react-icons/bs";
import { TbMist } from "react-icons/tb";
import { WiDust, WiSandstorm, WiSmoke, WiTornado } from "react-icons/wi";
import { VscError } from "react-icons/vsc";
import { TiWeatherStormy } from "react-icons/ti";

export const MyWeatherIcon = ({ weatherCondition, size }) => {
  switch (weatherCondition) {
    case 0:
      return <IoThunderstormOutline size={size} />; //Thunderstorm
    case 1:
      return <BsCloudDrizzle size={size} />; //Drizzle
    case 2:
      return <IoRainyOutline size={size} />; //Rain
    case 3:
      return <IoSnowOutline size={size} />; //Snow
    case 4:
      return <TbMist size={size} />; //Mist
    case 5:
      return <WiSmoke size={size} />; //Smoke
    case 6:
      return <BsCloudHaze2 size={size} />; //Haze
    case 7:
      return <WiDust size={size} />; //Dust
    case 8:
      return <BsCloudFog2 size={size} />; //Fog
    case 9:
      return <WiSandstorm size={size} />; //Sand
    case 10:
      return <WiDust size={size} />; //Ash
    case 11:
      return <TiWeatherStormy size={size} />; //Squall
    case 12:
      return <WiTornado size={size} />; //Tornado
    case 13:
      return <IoSunnyOutline size={size} />; //clear
    case 14:
      return <IoCloudyOutline size={size} />; //clouds
    default:
      return <VscError size={size} />;
  }
};
