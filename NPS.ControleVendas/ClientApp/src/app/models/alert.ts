
export interface Alert {
  type: string;
  show: boolean;
  header: string;
  message: string;
  timeToHide:number; //tempo para esconder a popup , -1 se ele deve permanecer a mostra
}
