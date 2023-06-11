import{_ as C,o as r,c as a,a as o,w as G,d as f,v as D,F as g,r as v,t as u,e as y,b as m,f as F,p as _,g as b,n as I,h as $,i as p,C as k,j as c,k as w,L as O}from"./index-6bf7cb31.js";const U={props:{document:{type:Object,required:!0},guids:{type:Array,required:!0},documents:{type:Array,required:!0},folders:{type:Array},folder:{type:String,default:""},validation:{type:Object,default:()=>{}}},data(){return{show:!1,editedDocument:{}}},mounted(){this.editedDocument.folder=this.document.folder},watch:{"editedDocument.guid":{immediate:!0,handler(t){const e=this.documents.find(n=>n.guid===t);e&&(this.editedDocument.title=e.title,this.editedDocument.content=e.content,this.editedDocument.tags=e.tags,this.editedDocument.version=e.version,this.editedDocument.folder=e.folder)}}},methods:{open(){this.editedDocument={...this.document},this.show=!0},close(){this.show=!1},closeOnOverlayClick(t){t.target===t.currentTarget&&(this.close(),this.clearError())},cancel(){this.close(),this.clearError()},submit(){this.$emit("document-updated",this.editedDocument),this.clearError()},async updateContent(){this.clearError();const t=this.documents.find(e=>e.guid===this.editedDocument.guid);t&&(this.editedDocument.content=t.content)},clearError(){this.$emit("clear-validation")}}},h=t=>(_("data-v-5a57789a"),t=t(),b(),t),A={class:"edit-dialog"},T=h(()=>o("h2",null,"Edit Document",-1)),j=h(()=>o("label",{for:"guid"},"GUID:",-1)),M=["value"],B=h(()=>o("label",{for:"title"},"Title:",-1)),N={key:0,class:"error"},q=h(()=>o("label",{for:"content"},"Content:",-1)),L=h(()=>o("label",{for:"tags"},"Tags:",-1)),R={key:0,class:"error"},W={for:"version"},z=["value"],H=["value"],K={class:"edit-dialog-actions"},J=h(()=>o("button",{type:"submit"},"Save",-1));function P(t,e,n,d,l,s){return l.show?(r(),a("div",{key:0,class:"edit-dialog-overlay",onClick:e[10]||(e[10]=i=>s.closeOnOverlayClick(i))},[o("div",A,[T,o("form",{onSubmit:e[9]||(e[9]=G((...i)=>s.submit&&s.submit(...i),["prevent"]))},[j,f(o("select",{id:"guid","onUpdate:modelValue":e[0]||(e[0]=i=>l.editedDocument.guid=i),class:"custom-select",onChange:e[1]||(e[1]=(...i)=>s.updateContent&&s.updateContent(...i))},[(r(!0),a(g,null,v(n.guids,i=>(r(),a("option",{key:i,value:i},u(i),9,M))),128))],544),[[D,l.editedDocument.guid]]),B,o("div",null,[f(o("input",{type:"text",id:"title","onUpdate:modelValue":e[2]||(e[2]=i=>l.editedDocument.title=i),onInput:e[3]||(e[3]=(...i)=>s.clearError&&s.clearError(...i))},null,544),[[y,l.editedDocument.title]]),n.validation.title?(r(),a("div",N,u(n.validation.title),1)):m("",!0)]),q,f(o("textarea",{id:"content","onUpdate:modelValue":e[4]||(e[4]=i=>l.editedDocument.content=i)},null,512),[[y,l.editedDocument.content]]),L,o("div",null,[f(o("input",{type:"text",id:"tags","onUpdate:modelValue":e[5]||(e[5]=i=>l.editedDocument.tags=i),onInput:e[6]||(e[6]=(...i)=>s.clearError&&s.clearError(...i))},null,544),[[y,l.editedDocument.tags]]),n.validation.tags?(r(),a("div",R,u(n.validation.tags),1)):m("",!0)]),o("p",null,[o("label",W,[F("Version: "),o("span",null,"v."+u(l.editedDocument.version),1)])]),f(o("select",{id:"folder","onUpdate:modelValue":e[7]||(e[7]=i=>l.editedDocument.folder=i),class:"custom-select"},[n.folders.length?m("",!0):(r(),a("option",{key:0,value:t.defaultFolder},u(this.folder),9,z)),(r(!0),a(g,null,v(n.folders,i=>(r(),a("option",{key:i,value:i},u(i.name),9,H))),128))],512),[[D,l.editedDocument.folder]]),o("div",K,[o("button",{type:"button",onClick:e[8]||(e[8]=(...i)=>s.cancel&&s.cancel(...i))},"Cancel"),J])],32)])])):m("",!0)}const E=C(U,[["render",P],["__scopeId","data-v-5a57789a"]]);const V=t=>(_("data-v-800d53e2"),t=t(),b(),t),Q={class:"document-container"},X={class:"document"},Y=["title"],Z=["title"],x=V(()=>o("i",{class:"fa-solid fa-tag tag-icon"},null,-1)),ee={key:0,class:"tag-list"},te=["title"],oe=V(()=>o("div",{class:"actions"},[o("span",{class:"loadContent"},[o("i",{class:"fa-solid fa-file"})])],-1)),ne={class:"documentContent"},ie={class:"document-actions"},de={props:{document:{type:Object,required:!0},documents:{type:Array,required:!0}},data(){return{loading:!1,loadedDocument:null,hover:!1,showContent:!1,guids:[],folders:[],validation:{}}},components:{ConfirmDialog:k,EditDocumentDialog:E},created(){this.fetchFolders()},methods:{clearValidation(){this.validation={}},getFolderNameByDocumentGuid(t){const e=this.folders.find(n=>n.documents.some(d=>d.guid===t));return e?e.name:""},async fetchFolders(){try{this.folders=(await c.get("/folders")).data}catch{alert("Error fetching folders")}},async loadContent(t){if(!this.loading)try{this.loading=!0,this.loadedDocument=(await c.get(`folders/${this.folderGuid}/${t}`)).data,this.showContent=!this.showContent}catch{}finally{this.loading=!1}},getDocument(t){return this.loadedDocument&&this.loadedDocument.guid===t?this.loadedDocument.content:null},async confirmDelete(){if(await this.$refs.confirmDialog.open())try{await c.delete(`documents/${this.document.guid}`),this.$emit("document-deleted",this.document.guid)}catch{}},async editDocument(){this.$refs.editDialog.open()},async updateDocument(t){try{const e=t.tags?await this.fetchTags(t.tags):[],n={guid:t.guid,title:t.title,content:t.content,type:t.type,tags:e,version:t.version+1};await c.put(`documents/${t.guid}`,n);const l=(await c.get(`folders/${t.folder.guid}/`)).data.documents.map(i=>i.title);l.includes(t.title)||l.push(t.title);const s={guid:t.folder.guid,name:t.folder.name,DocumentTitles:l};await c.put(`folders/${t.folder.guid}`,s),this.$emit("document-updated",n,!0),this.$refs.editDialog.close(),this.fetchFolders()}catch(e){e.response?this.validation=Object.keys(e.response.data.errors).reduce((n,d)=>{const l=d.charAt(0).toLowerCase()+d.slice(1);return n[l]=e.response.data.errors[d][0],n},{}):this.validation="Error when editing document"}},async fetchTags(t){try{let e=[];typeof t=="string"&&t.includes(",")?e=t.split(",").map(s=>s.trim()):e=Array.isArray(t)?t:[t];const n=await c.get("tags"),d=new Map(n.data.map(s=>[s.name,s.id]));return e.map(s=>({tagId:d.get(s)}))}catch(e){alert("Error fetching tags:",e)}},async fetchGuids(){if(!this.loading)try{const t=await c.get(`folders/${this.folderGuid}`);this.guids=t.data.documents.map(e=>e.guid)}catch{}finally{this.loading=!1}}},computed:{folderGuid(){return this.$route.params.id},versionStyle(){return this.document.tags.length>0?{}:{flexGrow:1}}},mounted(){this.fetchGuids()}},se=Object.assign(de,{__name:"DocumentComponent",setup(t){return(e,n)=>(r(),a(g,null,[o("div",Q,[o("div",X,[o("div",{class:"documentsHeader",onClick:n[2]||(n[2]=d=>e.loadContent(t.document.guid))},[o("div",{class:"title",title:t.document.title},u(t.document.title),9,Y),o("div",{class:"version",title:t.document.version,style:I(e.versionStyle)}," v."+u(t.document.version),13,Z),t.document.tags.length>0&&!e.show?(r(),a("div",{key:0,class:"tags",onMouseover:n[0]||(n[0]=d=>e.hover=!0),onMouseleave:n[1]||(n[1]=d=>e.hover=!1)},[x,e.hover?(r(),a("div",ee,[o("ul",null,[(r(!0),a(g,null,v(t.document.tags,d=>(r(),a("li",{key:d},u(d),1))),128))])])):m("",!0)],32)):m("",!0),o("div",{class:"type",title:t.document.type},"."+u(t.document.type),9,te),oe]),o("div",ne,[f(o("div",null,u(e.getDocument(t.document.guid)),513),[[$,e.showContent]])])]),o("div",ie,[o("button",{class:"edit-button",onClick:n[3]||(n[3]=(...d)=>e.editDocument&&e.editDocument(...d))},"Edit"),o("button",{class:"delete-button",onClick:n[4]||(n[4]=(...d)=>e.confirmDelete&&e.confirmDelete(...d))},"Delete")])]),p(k,{ref:"confirmDialog",title:"Delete Document",message:"Are you sure you want to delete this document?"},null,512),p(E,{ref:"editDialog",document:t.document,guids:e.guids,documents:t.documents,folders:e.folders,folder:e.getFolderNameByDocumentGuid(t.document.guid),onDocumentUpdated:e.updateDocument,onClearValidation:e.clearValidation,validation:e.validation},null,8,["document","guids","documents","folders","folder","onDocumentUpdated","onClearValidation","validation"])],64))}}),S=C(se,[["__scopeId","data-v-800d53e2"]]),le={class:"folderView"},re={class:"folders"},ae={key:0,class:"documents"},ue={components:{DocumentComponent:S},data(){return{folder:{},loading:!1}},methods:{async loadFolder(){this.loading=!0;try{this.folder=(await c.get(`folders/${this.folderGuid}`)).data}catch{alert("Server was unable to load folder.")}finally{this.loading=!1}}},async mounted(){this.loadFolder(),this.loading=!0},computed:{folderGuid(){return this.$route.params.id}}},me=Object.assign(ue,{__name:"FolderView",setup(t){return(e,n)=>(r(),a("div",le,[e.loading?(r(),w(O,{key:0})):m("",!0),o("h1",null,u(e.folder.name),1),o("div",re,[o("h4",null,"guid: "+u(e.folderGuid),1),e.folder.documents?(r(),a("div",ae,[(r(!0),a(g,null,v(e.folder.documents,d=>(r(),w(S,{key:d.guid,document:d,documents:e.folder.documents,onDocumentDeleted:e.loadFolder,onDocumentUpdated:e.loadFolder},null,8,["document","documents","onDocumentDeleted","onDocumentUpdated"]))),128))])):m("",!0)])]))}});export{me as default};