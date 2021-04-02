import { Photo } from "./photo";

export interface Member {
    id: number;
    age: number;
    userName: string;
    photoUrl: string;
    knownAs: string;
    gender: string;
    introduction: string;
    lookingFor: string;
    interests: string;
    city: string;
    country: string;
    photos: Photo[];
    created: Date;
    lastActive: Date;
  }
  
