import{_ as c,o as n,c as i,a as o,t as l,b as _}from"./index-711bc678.js";const p="/assets/spengerlogo-5084c112.svg";const r={data(){return{showPopup:!1,copiedText:""}},methods:{copyEmail(e){navigator.clipboard.writeText(e).then(()=>{this.copiedText=e,this.showPopup=!0,setTimeout(()=>{this.showPopup=!1},2e3)})}}},d={class:"aboutViewContainer"},u=o("h1",null,"About",-1),h=o("p",null,null,-1),m=o("h4",null,"Made By:",-1),g=o("div",{class:"name"},"Tristan Losada Benini",-1),v=o("i",{class:"fas fa-envelope"},null,-1),f=[v],b=o("p",null,"(in collaboration with Ruben Osmanovic)",-1),w=o("img",{id:"spengerlogo",src:p},null,-1),x={key:0,class:"popup"};function y(e,s,T,B,t,a){return n(),i("div",d,[u,h,m,g,o("div",{onClick:s[0]||(s[0]=V=>a.copyEmail("LOS20421@spengergasse.at"))},f),b,w,t.showPopup?(n(),i("div",x," Copied to clipboard: "+l(t.copiedText),1)):_("",!0)])}const C=c(r,[["render",y]]);export{C as default};
