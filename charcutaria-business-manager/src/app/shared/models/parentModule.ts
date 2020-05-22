export interface ParentModule extends ChildModule {
    childModules: ChildModule[];
}
export interface ChildModule {
    name: string;
    route: string;
    moduleId: Number;
}